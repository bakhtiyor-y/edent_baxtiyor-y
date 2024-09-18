using Edent.Api.Infrastructure.Filters;
using Edent.Api.Models;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IScheduleSettingService _scheduleSettingService;
        private readonly IDoctorService _doctorService;
        private readonly IUserResolverService _userResolverService;
        private readonly IAppointmentService _appointmentService;

        public ScheduleController(IScheduleService scheduleService,
            IScheduleSettingService scheduleSettingService,
            IDoctorService doctorService,
            IUserResolverService userResolverService,
            IAppointmentService appointmentService)
        {
            _scheduleService = scheduleService;
            _scheduleSettingService = scheduleSettingService;
            _doctorService = doctorService;
            _userResolverService = userResolverService;
            _appointmentService = appointmentService;
        }


        [HttpPost]
        public IActionResult Post(ScheduleViewModel model)
        {
            if (model.DoctorId == 0)
            {
                var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
                if (userId == 0)
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
                }

                var doctor = _doctorService
                    .Query()
                    .FirstOrDefault(f => f.UserId == userId);

                if (doctor == null)
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor not found" } });
                }
                model.DoctorId = doctor.Id;
            }
            var schedule = _scheduleService.Create(model);
            _scheduleSettingService.SaveScheduleSettings(schedule.Id, model.ScheduleSettings);

            if (schedule != null)
                return Ok(new ApiResultModel<bool> { Result = true });

            return BadRequest(new ApiResultModel<bool> { Result = false });
        }

        [HttpPut]
        public IActionResult Put(ScheduleViewModel model)
        {
            var schedule = _scheduleService.Update(model.Id, model);
            _scheduleSettingService.SaveScheduleSettings(model.Id, model.ScheduleSettings);

            if (schedule != null)
                return Ok(new ApiResultModel<bool> { Result = true });

            return BadRequest(new ApiResultModel<bool> { Result = false });
        }

        [HttpGet("GetSchedule")]
        public IActionResult GetSchedule()
        {
            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
            if (userId == 0)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }

            var doctor = _doctorService
                .Query()
                .FirstOrDefault(f => f.UserId == userId);
            if (doctor == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor not found" } });
            }

            var schedule = _scheduleService
               .Query()
               .Include("ScheduleSettings.SettingDayOfWeeks")
               .FirstOrDefault(w => w.DoctorId == doctor.Id);

            if (schedule == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Schedule not found" } });
            }

            var result = _scheduleService.Mapper.Map<ScheduleViewModel>(schedule);
            return Ok(result);
        }

        [HttpGet("GetScheduleByDoctor")]
        public IActionResult GetScheduleByDoctor(int doctorId)
        {

            var doctor = _doctorService.GetById(doctorId);
            if (doctor == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor not found" } });
            }

            var schedule = _scheduleService
                .Query()
                .Include("ScheduleSettings.SettingDayOfWeeks")
                .FirstOrDefault(w => w.DoctorId == doctorId);

            if (schedule == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Schedule not found" } });
            }

            var result = _scheduleService.Mapper.Map<ScheduleViewModel>(schedule);
            return Ok(result);
        }


        [HttpGet("GetDoctorSchedule")]
        public IActionResult GetDoctorSchedule(int doctorId, long date)
        {
            var appointmentDate = DateTimeOffset.FromUnixTimeMilliseconds(date);
            var doctor = _doctorService.GetById(doctorId);
            if (doctor == null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor not found!" } });

            var schedule = _scheduleService.GetScheduleBy(doctor.Id);
            if (schedule == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Schedule not found" } });
            }

            ICollection<ScheduleEventModel> scheduleEventModels = new HashSet<ScheduleEventModel>();
            var scheduleSetting = schedule.ScheduleSettings.FirstOrDefault(f => f.SettingDayOfWeeks.Any(a => a.DayOfWeek == appointmentDate.LocalDateTime.DayOfWeek));
            if (scheduleSetting == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor does'nt work in selected date" } });
            }
            var doctorAppointments = _appointmentService.GetAppointmentsBy(doctor.Id, appointmentDate);
            var jointAppointments = _appointmentService.GetJointAppointmentsBy(doctor.Id, appointmentDate);
            var admissionCount = (scheduleSetting.ToMinute - scheduleSetting.FromMinute).TotalMinutes / schedule.AdmissionDuration;
            for (double i = 0; i < admissionCount; i++)
            {

                var fromTimeSpan = scheduleSetting.FromMinute.Add(new TimeSpan(0, (int)(schedule.AdmissionDuration * i), 0) - DateTimeOffset.Now.Offset);
                var currentAppointmentDate = new DateTimeOffset(appointmentDate.LocalDateTime.Year, appointmentDate.LocalDateTime.Month, appointmentDate.LocalDateTime.Day, fromTimeSpan.Hours, fromTimeSpan.Minutes, 0, TimeSpan.Zero);
                ScheduleEventModel scheduleEventModel = new ScheduleEventModel();
                if (doctorAppointments.Any(a => a.AppointmentDate == currentAppointmentDate
                                                 && a.AppointmentStatus != Infrastructure.Enums.AppointmentStatus.Cancelled) ||
                    jointAppointments.Any(a => a.AppointmentDate == currentAppointmentDate
                                                 && a.AppointmentStatus != Infrastructure.Enums.AppointmentStatus.Cancelled))
                {
                    scheduleEventModel.IsBusy = true;
                    scheduleEventModel.Name = "Занято";
                    scheduleEventModel.Description = "На это время имеется запись на прием.";
                }
                else
                {
                    scheduleEventModel.IsBusy = false;
                    scheduleEventModel.Name = "Свободно";
                    scheduleEventModel.Description = "Время доступно для записи на прием";
                }
                scheduleEventModel.Starting = currentAppointmentDate;
                scheduleEventModels.Add(scheduleEventModel);
            }
            return Ok(scheduleEventModels);
        }

        [HttpGet("GetScheduleByDate")]
        public IActionResult GetScheduleByDate(long date)
        {
            var appointmentDate = DateTimeOffset.FromUnixTimeMilliseconds(date);
            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
            if (userId == 0)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }

            var doctor = _doctorService
                .Query()
                .FirstOrDefault(f => f.UserId == userId);
            if (doctor == null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor not found!" } });

            var schedule = _scheduleService.GetScheduleBy(doctor.Id);
            if (schedule == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Schedule not found" } });
            }

            ICollection<ScheduleEventModel> scheduleEventModels = new HashSet<ScheduleEventModel>();
            var scheduleSetting = schedule.ScheduleSettings.FirstOrDefault(f => f.SettingDayOfWeeks.Any(a => a.DayOfWeek == appointmentDate.LocalDateTime.DayOfWeek));
            if (scheduleSetting == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor does'nt work in selected date" } });
            }
            var doctorAppointments = _appointmentService.GetAppointmentsBy(doctor.Id, appointmentDate);
            var jointAppointments = _appointmentService.GetJointAppointmentsBy(doctor.Id, appointmentDate);
            var admissionCount = (scheduleSetting.ToMinute - scheduleSetting.FromMinute).TotalMinutes / schedule.AdmissionDuration;
            for (double i = 0; i < admissionCount; i++)
            {
                var fromTimeSpan = scheduleSetting.FromMinute.Add(new TimeSpan(0, (int)(schedule.AdmissionDuration * i), 0) - DateTimeOffset.Now.Offset);
                var currentAppointmentDate = new DateTimeOffset(appointmentDate.LocalDateTime.Year, appointmentDate.LocalDateTime.Month, appointmentDate.LocalDateTime.Day, fromTimeSpan.Hours, fromTimeSpan.Minutes, 0, TimeSpan.Zero);
                ScheduleEventModel scheduleEventModel = new();
                if (doctorAppointments.Any(a => a.AppointmentDate == currentAppointmentDate
                                                 && a.AppointmentStatus != Infrastructure.Enums.AppointmentStatus.Cancelled) ||
                   jointAppointments.Any(a => a.AppointmentDate == currentAppointmentDate
                                                 && a.AppointmentStatus != Infrastructure.Enums.AppointmentStatus.Cancelled))
                {
                    scheduleEventModel.IsBusy = true;
                    scheduleEventModel.Name = "Занято";
                    scheduleEventModel.Description = "На это время имеется запись на прием.";
                }
                else
                {
                    scheduleEventModel.IsBusy = false;
                    scheduleEventModel.Name = "Свободно";
                    scheduleEventModel.Description = "Время доступно для записи на прием";
                }
                scheduleEventModel.Starting = currentAppointmentDate;
                scheduleEventModels.Add(scheduleEventModel);
            }
            return Ok(scheduleEventModels);
        }

    }
}
