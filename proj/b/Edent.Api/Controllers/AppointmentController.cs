using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Enums;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.Models;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserResolverService _userResolverService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly UserManager<User> _userManager;
        private readonly IAddressService _addressService;
        private readonly IPatientAddressService _patientAddressService;
        private readonly IScheduleService _scheduleService;
        private readonly IPatientToothService _patientToothService;

        public AppointmentController(IAppointmentService appointmentService,
            IUserResolverService userResolverService,
            IDoctorService doctorService,
            IPatientService patientService,
            UserManager<User> userManager,
            IAddressService addressService,
            IPatientAddressService patientAddressService,
            IScheduleService scheduleService,
            IPatientToothService patientToothService)
        {
            _appointmentService = appointmentService;
            _userResolverService = userResolverService;
            _doctorService = doctorService;
            _patientService = patientService;
            _userManager = userManager;
            _addressService = addressService;
            _patientAddressService = patientAddressService;
            _scheduleService = scheduleService;
            _patientToothService = patientToothService;
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Get()
        {

            return Ok();
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var query = _appointmentService
                .Query()
                .Include("Doctor.User")
                .Include("Patient")
                .FirstOrDefault(f => f.Id == id);

            var result = _patientService.Mapper.Map<AppointmentViewModel>(query);
            return Ok(result);
        }


        [HttpGet("GetReceptionAppointments")]
        [Authorize(Roles = "reception, admin")]
        public IActionResult GetReceptionAppointments([FromQuery] string filter, [FromQuery] string name = null, [FromQuery] int appointmentStatus = 99)
        {

            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _appointmentService
                .Query()
                .Include("Doctor.User")
                .Include("Patient")
                .Where(w => w.EmployeeId == null);

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lname = name.ToLower();
                query = query.Where(w => w.Patient.FirstName.ToLower().Contains(lname) || w.Patient.LastName.ToLower().Contains(lname));
            }

            if (appointmentStatus != 99)
            {
                var status = (AppointmentStatus)appointmentStatus;
                query = query.Where(w => w.AppointmentStatus == status);
            }

            query = query.OrderByDescending(o => o.AppointmentDate)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _patientService.Mapper.Map<IEnumerable<AppointmentViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpGet("GetDoctorAppointments")]
        [Authorize(Roles = "doctor, admin")]
        public IActionResult GetDoctorAppointments(long date)
        {
            var appointmentDate = DateTimeOffset.FromUnixTimeMilliseconds(date);
            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
            if (userId == 0)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }

            var doctor = _doctorService.Query().FirstOrDefault(f => f.UserId == userId);
            if (doctor == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor not found" } });
            }

            var schedule = _scheduleService.Query().FirstOrDefault(f => f.DoctorId == doctor.Id);
            if (schedule == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Schedule not setted" } });
            }

            var result = _appointmentService.Query()
                .Include("Patient")
                .Include(i => i.JointDoctors)
                .Where(w => w.AppointmentDate.Year == appointmentDate.Year
                    && w.AppointmentDate.Month == appointmentDate.Month
                    && (w.DoctorId == doctor.Id || w.JointDoctors.Any(a => a.DoctorId == doctor.Id))
                    && w.AppointmentStatus != AppointmentStatus.Cancelled)
                .Select(s => new DoctorAppointmentViewModel
                {
                    Id = s.Id,
                    Title = $"{s.Patient.LastName} {s.Patient.FirstName} {s.Patient.Patronymic}",
                    Start = s.AppointmentDate,
                    End = s.AppointmentDate.AddMinutes(schedule.AdmissionDuration),
                    AppointmentStatus = s.AppointmentStatus,
                    IsJoint = s.DoctorId != doctor.Id
                }).ToList();

            return Ok(result);
        }

        [HttpGet("GetRentgenAppointments")]
        [Authorize(Roles = "rentgen, admin")]
        public IActionResult GetRentgenAppointments([FromQuery] string filter, [FromQuery] string name = null, [FromQuery] int appointmentStatus = 99)
        {

            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _appointmentService
                .Query()
                .Include("Employee.User")
                .Include("Patient")
                .Where(w => w.EmployeeId != null && w.DoctorId == null);

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lname = name.ToLower();
                query = query.Where(w => w.Patient.FirstName.ToLower().Contains(lname) || w.Patient.LastName.ToLower().Contains(lname));
            }

            if (appointmentStatus != 99)
            {
                var status = (AppointmentStatus)appointmentStatus;
                if (status == AppointmentStatus.Appointed)
                {
                    query = query.Where(w => w.AppointmentStatus == AppointmentStatus.Appointed || w.AppointmentStatus == AppointmentStatus.Postponed);
                }
                else
                {
                    query = query.Where(w => w.AppointmentStatus == status);
                }
            }

            query = query.OrderByDescending(o => o.AppointmentDate)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _patientService.Mapper.Map<IEnumerable<AppointmentViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }


        [HttpGet("GetRentgenAppointmentsByDate")]
        [Authorize(Roles = "reception, rentgen, admin")]
        public IActionResult GetRentgenAppointmentsByDate(long date)
        {
            var appointmentDate = DateTimeOffset.FromUnixTimeMilliseconds(date);
            //var currentAppointmentDate = new DateTimeOffset(appointmentDate.LocalDateTime.Year, appointmentDate.LocalDateTime.Month, appointmentDate.LocalDateTime.Day, appointmentDate.LocalDateTime.Hour, appointmentDate.LocalDateTime.Minute, 0, TimeSpan.Zero);
            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
            if (userId == 0)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }

            var queryResult = _appointmentService.Query()
                .Include("Patient")
                .Where(w => w.AppointmentDate.Year == appointmentDate.Year
                    && w.AppointmentDate.Month == appointmentDate.Month
                    && w.AppointmentDate.Day == appointmentDate.Day
                    && w.DoctorId == null
                    && w.EmployeeId != null
                    && w.AppointmentStatus != AppointmentStatus.Cancelled)
                .ToList();
            var result = _appointmentService.Mapper.Map<IEnumerable<AppointmentViewModel>>(queryResult);

            return Ok(result);
        }


        [HttpPost("AppointByReception")]
        [Authorize(Roles = "reception, admin")]
        public async Task<IActionResult> AppointByReception(AppointmentManageViewModel viewModel)
        {
            bool result = false;

            if (viewModel.Patient == null)
                return Ok(new ApiResultModel<bool> { Result = result });

            PatientViewModel patient = viewModel.Patient.Id > 0 ?
                await _patientService.UpdatePatient(viewModel.Patient)
                : _patientService.CreatePatient(viewModel.Patient);

            if (patient.Id <= 0)
                return Ok(new ApiResultModel<bool> { Result = result });

            _patientToothService.InitPatientTeeth(patient.Id, viewModel.Patient.PatientAgeType);


            var appointment = new Appointment
            {
                Description = viewModel.Description,
                DoctorId = viewModel.DoctorId,
                EmployeeId = viewModel.EmployeeId,
                PatientId = patient.Id,
                PartnerId = viewModel.PartnerId,
                AppointmentStatus = viewModel.AppointmentStatus,
                AppointmentDate = viewModel.AppointmentDate,
                DentalChairId = viewModel.DentalChairId
            };
            foreach (var dId in viewModel.JointDoctors)
            {
                var doctorAppointments = _appointmentService.GetAppointmentsBy(dId, viewModel.AppointmentDate);
                var jointAppointments = _appointmentService.GetJointAppointmentsBy(dId, viewModel.AppointmentDate);
                if (doctorAppointments.Any(a => a.AppointmentDate == viewModel.AppointmentDate
                                         && a.AppointmentStatus != AppointmentStatus.Cancelled) ||
                    jointAppointments.Any(a => a.AppointmentDate == viewModel.AppointmentDate
                                         && a.AppointmentStatus != AppointmentStatus.Cancelled))
                {
                    return BadRequest(new { Message = "Участвующий врач в совместном приёме занят в выбранное время" });
                }
                appointment.JointDoctors.Add(new JointDoctor { DoctorId = dId });
            }
            var createdAppointment = _appointmentService.Create(appointment);
            result = createdAppointment.Id != 0;

            return Ok(new ApiResultModel<bool> { Result = result });
        }

        [HttpPost("AppointByDoctor")]
        [Authorize(Roles = "doctor, admin")]
        public async Task<IActionResult> AppointByDoctor(AppointmentManageViewModel viewModel)
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
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor not found!" } });

            bool result = false;
            if (viewModel.Patient == null)
            {
                return Ok(new ApiResultModel<bool> { Result = result });
            }

            PatientViewModel patient = viewModel.Patient.Id > 0 ?
                await _patientService.UpdatePatient(viewModel.Patient) :
                _patientService.CreatePatient(viewModel.Patient);

            _patientToothService.InitPatientTeeth(patient.Id, viewModel.Patient.PatientAgeType);

            if (patient.Id > 0)
            {
                var appointment = new Appointment
                {
                    Description = viewModel.Description,
                    DoctorId = doctor.Id,
                    EmployeeId = viewModel.EmployeeId,
                    PatientId = patient.Id,
                    PartnerId = viewModel.PartnerId,
                    AppointmentStatus = viewModel.AppointmentStatus,
                    AppointmentDate = viewModel.AppointmentDate,
                    DentalChairId = viewModel.DentalChairId
                };
                foreach (var dId in viewModel.JointDoctors)
                {
                    var doctorAppointments = _appointmentService.GetAppointmentsBy(dId, viewModel.AppointmentDate);
                    var jointAppointments = _appointmentService.GetJointAppointmentsBy(dId, viewModel.AppointmentDate);
                    if (doctorAppointments.Any(a => a.AppointmentDate == viewModel.AppointmentDate
                                             && a.AppointmentStatus != AppointmentStatus.Cancelled) ||
                        jointAppointments.Any(a => a.AppointmentDate == viewModel.AppointmentDate
                                             && a.AppointmentStatus != AppointmentStatus.Cancelled))
                    {
                        return BadRequest(new { Message = "Участвующий врач в совместном приёме занят в выбранное время" });
                    }
                    appointment.JointDoctors.Add(new JointDoctor { DoctorId = dId });
                }
                var createdAppointment = _appointmentService.Create(appointment);
                result = createdAppointment.Id != 0;
            }

            return Ok(new ApiResultModel<bool> { Result = result });
        }

        [HttpPost("AppointToRentgen")]
        [Authorize(Roles = "reception, admin")]
        public async Task<IActionResult> AppointToRentgen(AppointmentManageViewModel viewModel)
        {
            bool result = false;

            if (viewModel.Patient == null)
                return Ok(new ApiResultModel<bool> { Result = result });

            PatientViewModel patient = viewModel.Patient.Id > 0 ?
                await _patientService.UpdatePatient(viewModel.Patient)
                : _patientService.CreatePatient(viewModel.Patient);

            if (patient.Id <= 0)
                return Ok(new ApiResultModel<bool> { Result = result });
            _patientToothService.InitPatientTeeth(patient.Id, viewModel.Patient.PatientAgeType);

            var appointment = new Appointment
            {
                Description = viewModel.Description,
                DoctorId = viewModel.DoctorId,
                EmployeeId = viewModel.EmployeeId,
                PatientId = patient.Id,
                PartnerId = viewModel.PartnerId,
                AppointmentStatus = viewModel.AppointmentStatus,
                AppointmentDate = viewModel.AppointmentDate,
                DentalChairId = viewModel.DentalChairId
            };

            var createdAppointment = _appointmentService.Create(appointment);
            result = createdAppointment.Id != 0;

            return Ok(new ApiResultModel<bool> { Result = result });
        }


        [HttpPut("EditAppointByReception")]
        [Authorize(Roles = "reception, admin")]
        public IActionResult EditAppointByReception(AppointmentDateEditViewModel viewModel)
        {
            var appointment = _appointmentService.Query()
                .Include(i => i.JointDoctors)
                .FirstOrDefault(f => f.Id == viewModel.Id);

            if (appointment == null)
            {
                return BadRequest(new JsonErrorResponse
                {
                    Messages = new[] { "Appointment not found" }
                });
            }

            foreach (var d in appointment.JointDoctors)
            {
                var doctorAppointments = _appointmentService.GetAppointmentsBy(d.DoctorId, viewModel.AppointmentDate);
                var jointAppointments = _appointmentService.GetJointAppointmentsBy(d.DoctorId, viewModel.AppointmentDate);
                if (doctorAppointments.Any(a => a.AppointmentDate == viewModel.AppointmentDate
                                         && a.AppointmentStatus != AppointmentStatus.Cancelled) ||
                    jointAppointments.Any(a => a.AppointmentDate == viewModel.AppointmentDate
                                         && a.AppointmentStatus != AppointmentStatus.Cancelled))
                {
                    return BadRequest(new { Message = "Участвующий врач в совместном приёме занят в выбранное время" });
                }
            }

            appointment.PartnerId = viewModel.PartnerId;
            appointment.AppointmentStatus = AppointmentStatus.Postponed;
            appointment.AppointmentDate = viewModel.AppointmentDate;
            appointment.DentalChairId = viewModel.DentalChairId;

            _appointmentService.Repository.Edit(appointment);
            var saveResult = _appointmentService.Repository.SaveChanges();
            if (saveResult)
            {
                var updated = _appointmentService
                       .Query()
                       .Include("Doctor.User")
                       .Include("Patient")
                       .FirstOrDefault(w => w.Id == appointment.Id);

                var result = _patientService.Mapper.Map<AppointmentViewModel>(updated);
                return Ok(result);
            }
            else
            {
                return BadRequest(new JsonErrorResponse
                {
                    Messages = new[] { "Error on save appointment" }
                });
            }
        }

        [HttpPut("EditAppointByDoctor")]
        [Authorize(Roles = "doctor, admin")]
        public IActionResult EditAppointByDoctor(AppointmentDateEditViewModel viewModel)
        {
            var appointment = _appointmentService.Query()
                .Include(i => i.JointDoctors)
                .FirstOrDefault(f => f.Id == viewModel.Id);

            if (appointment == null)
            {
                return BadRequest(new JsonErrorResponse
                {
                    Messages = new[] { "Appointment not found" }
                });
            }
            foreach (var d in appointment.JointDoctors)
            {
                var doctorAppointments = _appointmentService.GetAppointmentsBy(d.DoctorId, viewModel.AppointmentDate);
                var jointAppointments = _appointmentService.GetJointAppointmentsBy(d.DoctorId, viewModel.AppointmentDate);
                if (doctorAppointments.Any(a => a.AppointmentDate == viewModel.AppointmentDate
                                         && a.AppointmentStatus != AppointmentStatus.Cancelled) ||
                    jointAppointments.Any(a => a.AppointmentDate == viewModel.AppointmentDate
                                         && a.AppointmentStatus != AppointmentStatus.Cancelled))
                {
                    return BadRequest(new { Message = "Участвующий врач в совместном приёме занят в выбранное время" });
                }
            }
            appointment.PartnerId = viewModel.PartnerId;
            appointment.AppointmentStatus = AppointmentStatus.Postponed;
            appointment.AppointmentDate = viewModel.AppointmentDate;
            appointment.DentalChairId = viewModel.DentalChairId;

            _appointmentService.Repository.Edit(appointment);
            var saveResult = _appointmentService.Repository.SaveChanges();
            if (saveResult)
            {
                var updated = _appointmentService
                       .Query()
                       .Include("Doctor.User")
                       .Include("Patient")
                       .FirstOrDefault(w => w.Id == appointment.Id);

                var result = _patientService.Mapper.Map<AppointmentViewModel>(updated);
                return Ok(result);
            }
            else
            {
                return BadRequest(new JsonErrorResponse
                {
                    Messages = new[] { "Error on save appointment" }
                });
            }
        }

        [HttpGet("CancelByReception")]
        [Authorize(Roles = "reception, admin")]
        public IActionResult CancelByReception(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
            {
                return BadRequest(new JsonErrorResponse
                {
                    Messages = new[] { "Appointment not found" }
                });
            }
            appointment.AppointmentStatus = AppointmentStatus.Cancelled;
            _appointmentService.Repository.Edit(appointment);
            return Ok(_appointmentService.Repository.SaveChanges());
        }

        [HttpGet("CancelByDoctor")]
        [Authorize(Roles = "doctor, admin")]
        public IActionResult CancelByDoctor(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
            {
                return BadRequest(new JsonErrorResponse
                {
                    Messages = new[] { "Appointment not found" }
                });
            }
            appointment.AppointmentStatus = AppointmentStatus.Cancelled;
            _appointmentService.Repository.Edit(appointment);
            return Ok(_appointmentService.Repository.SaveChanges());
        }

        [HttpGet("CancelByRentgen")]
        [Authorize(Roles = "rentgen, admin")]
        public IActionResult CancelByRentgen(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
            {
                return BadRequest(new JsonErrorResponse
                {
                    Messages = new[] { "Appointment not found" }
                });
            }
            appointment.AppointmentStatus = AppointmentStatus.Cancelled;
            _appointmentService.Repository.Edit(appointment);
            return Ok(_appointmentService.Repository.SaveChanges());
        }
    }
}
