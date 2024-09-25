using Edent.Api.Infrastructure.Entities;
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
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly UserManager<User> _userManager;
        private readonly IOrganizationService _organizationService;
        private readonly IAddressService _addressService;
        private readonly IDoctorAddressService _doctorAddressService;
        private readonly IDoctorInTermService _doctorInTermService;
        private readonly IAppointmentService _appointmentService;
        private readonly IJointDoctorService _jointDoctorService;
        private readonly IDoctorDentalChairService _doctorDentalChairService;

        public DoctorController(IDoctorService doctorService,
            UserManager<User> userManager,
            IOrganizationService organizationService,
            IAddressService addressService,
            IDoctorAddressService doctorAddressService,
            IDoctorInTermService doctorInTermService,
            IAppointmentService appointmentService,
            IJointDoctorService jointDoctorService,
            IDoctorDentalChairService doctorDentalChairService)
        {
            _doctorService = doctorService;
            _userManager = userManager;
            _organizationService = organizationService;
            _addressService = addressService;
            _doctorAddressService = doctorAddressService;
            _doctorInTermService = doctorInTermService;
            _appointmentService = appointmentService;
            _jointDoctorService = jointDoctorService;
            _doctorDentalChairService = doctorDentalChairService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _doctorService
                .Query()
                .Include(i => i.User)
                .Include(i => i.Specialization)
                .Include("DoctorInTerms.Term")
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _doctorService.Mapper.Map<IEnumerable<DoctorViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var doctor = _doctorService
                .Query()
                .Include(i => i.User)
                .Include(i => i.Specialization)
                .Include("DoctorInTerms.Term")
                .Include("DoctorAddresses.Address.City.Region")
                .Include("DoctorDentalChairs")
                .FirstOrDefault(f => f.Id == id);

            var result = _doctorService.Mapper.Map<DoctorManageViewModel>(doctor);
            return Ok(result);
        }

        [HttpGet("GetByIdSimple")]
        public IActionResult GetByIdSimple(int id)
        {
            var doctor = _doctorService
                .Query()
                .Include(i => i.User)
                .Include(i => i.Specialization)
                .Include("DoctorInTerms.Term")
                .FirstOrDefault(f => f.Id == id);

            var result = _doctorService.Mapper.Map<DoctorViewModel>(doctor);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(DoctorManageViewModel viewModel)
        {
            User user = new User
            {
                IsActive = viewModel.IsActive,
                Email = viewModel.Email,
                EmailConfirmed = true,
                PhoneNumber = viewModel.PhoneNumber,
                PhoneNumberConfirmed = true,
                UserName = viewModel.Email
            };

            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "doctor");
                var organization = _organizationService.Query().FirstOrDefault();

                Doctor doctor = new Doctor
                {
                    BirthDate = viewModel.BirthDate,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Patronymic = viewModel.Patronymic,
                    OrganizationId = organization.Id,
                    SpecializationId = viewModel.SpecializationId,
                    Gender = viewModel.Gender,
                    DoctorInTerms = new List<DoctorInTerm>
                    {
                        new DoctorInTerm
                        {
                            TermId = viewModel.TermId,
                            TermValue = viewModel.TermValue
                        }
                    },
                    UserId = user.Id,
                    DoctorAddresses = new List<DoctorAddress>
                    {
                        new DoctorAddress
                        {
                            Address = new Address
                            {
                               AddressLine1 = viewModel.AddressLine1,
                               AddressLine2 = viewModel.AddressLine2,
                               CityId = viewModel.CityId
                            }
                        }
                    }
                };
                foreach (var id in viewModel.DentalChairs)
                {
                    doctor.DoctorDentalChairs.Add(new DoctorDentalChair { DentalChairId = id });
                }

                var createdDoctor = _doctorService.Create(doctor);
                var result = _doctorService.GetByIncluding(createdDoctor.Id);
                return Ok(result);
            }
            else
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on create new user" } });
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put(DoctorManageViewModel viewModel)
        {

            var user = await _userManager.FindByIdAsync(viewModel.UserId.ToString());
            user.IsActive = viewModel.IsActive;
            user.Email = viewModel.Email;
            user.PhoneNumber = viewModel.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                var doctor = _doctorService.GetById(viewModel.Id);
                doctor.SpecializationId = viewModel.SpecializationId;
                doctor.BirthDate = viewModel.BirthDate;
                doctor.FirstName = viewModel.FirstName;
                doctor.LastName = viewModel.LastName;
                doctor.Patronymic = viewModel.Patronymic;
                doctor.Gender = viewModel.Gender;
                _doctorService.Repository.Edit(doctor);
                await _doctorService.Repository.SaveChangesAsync();

                var doctorInTerm = _doctorInTermService
                    .Query()
                    .AsNoTracking()
                    .FirstOrDefault(f => f.DoctorId == doctor.Id) ?? new DoctorInTerm();

                doctorInTerm.DoctorId = viewModel.Id;
                doctorInTerm.TermId = viewModel.TermId;
                doctorInTerm.TermValue = viewModel.TermValue;

                if (doctorInTerm.Id > 0)
                    _doctorInTermService.Repository.Edit(doctorInTerm);
                else
                    _doctorInTermService.Repository.Add(doctorInTerm);

                await _doctorInTermService.Repository.SaveChangesAsync();


                var doctorAddress = _doctorAddressService
                    .Query()
                    .AsNoTracking()
                    .FirstOrDefault(f => f.DoctorId == doctor.Id) ?? new DoctorAddress();

                if (doctorAddress.Id == 0)
                {
                    doctorAddress.Address = new Address { CityId = viewModel.CityId, AddressLine1 = viewModel.AddressLine1, AddressLine2 = viewModel.AddressLine2 };
                    _doctorAddressService.Repository.Add(doctorAddress);
                    await _doctorAddressService.Repository.SaveChangesAsync();
                }
                else
                {
                    var address = _addressService.Query().AsNoTracking().FirstOrDefault(f => f.Id == doctorAddress.AddressId);
                    address.CityId = viewModel.CityId;
                    address.AddressLine1 = viewModel.AddressLine1;
                    address.AddressLine2 = viewModel.AddressLine2;
                    _addressService.Repository.Edit(address);
                    await _addressService.Repository.SaveChangesAsync();
                }

                _doctorDentalChairService.SaveDoctorDentalChairs(doctor.Id, viewModel.DentalChairs);

                var result = _doctorService.GetByIncluding(doctor.Id);
                return Ok(result);
            }
            else
            {
                return BadRequest(new JsonErrorResponse
                {
                    Messages = new[] { "Error on create new user" }
                });
            }
        }

        [HttpGet("SearchByName")]
        public IActionResult SearchByName(string name)
        {
            if (name == null)
            {
                return Ok(new DoctorViewModel[0]);
            }
            name = name.ToLower();

            var query = _doctorService
                .Query()
                .Include(i => i.User)
                .Include(i => i.Schedule)
                .Include(i => i.Specialization)
                .Where(w => (w.FirstName.ToLower().StartsWith(name) || w.LastName.ToLower().StartsWith(name))
                    && w.User.IsActive && w.Schedule != null);

            var result = _doctorService.Mapper.Map<IEnumerable<DoctorViewModel>>(query);
            return Ok(result);
        }

        [HttpGet("GetDoctorsWithSchedule")]
        public IActionResult GetDoctorsWithSchedule(long date)
        {
            var selectedDate = DateTimeOffset.FromUnixTimeMilliseconds(date);
            var yesterday = selectedDate.AddDays(-1);
            var tomorrow = selectedDate.AddDays(1);
            var doctors = _doctorService
                .Query()
                .Include(i => i.User)
                .Include(i => i.Schedule)
                    .ThenInclude(t => t.ScheduleSettings)
                        .ThenInclude(th => th.SettingDayOfWeeks)
                .Include(i => i.Appointments.Where(w => yesterday < w.AppointmentDate && tomorrow > w.AppointmentDate))
                    .ThenInclude(th => th.Patient)
                .Where(w => w.User.IsActive && w.Schedule != null)
                .ToList();

            ICollection<DoctorScheduleViewModel> doctorSchedules = new HashSet<DoctorScheduleViewModel>();

            foreach (var doctor in doctors)
            {
                if (doctor.Schedule == null)
                    continue;

                var scheduleSetting = doctor.Schedule.ScheduleSettings.FirstOrDefault(f => f.SettingDayOfWeeks.Any(a => a.DayOfWeek == selectedDate.LocalDateTime.DayOfWeek));
                if (scheduleSetting == null)
                    continue;

                DoctorScheduleViewModel doctorSchedule = new DoctorScheduleViewModel();
                doctorSchedule.DoctorId = doctor.Id;
                doctorSchedule.Name = $"{doctor.LastName} {doctor.FirstName} {doctor.Patronymic}";
                doctorSchedule.AdmissionDuration = doctor.Schedule.AdmissionDuration;
                var admissionCount = (scheduleSetting.ToMinute - scheduleSetting.FromMinute).TotalMinutes / doctor.Schedule.AdmissionDuration;
                for (double i = 0; i < admissionCount; i++)
                {
                    var fromTimeSpan = scheduleSetting.FromMinute.Add(new TimeSpan(0, (int)(doctor.Schedule.AdmissionDuration * i), 0) - DateTimeOffset.Now.Offset);
                    var currentAppointmentDate = new DateTimeOffset(selectedDate.LocalDateTime.Year, selectedDate.LocalDateTime.Month, selectedDate.LocalDateTime.Day, fromTimeSpan.Hours, fromTimeSpan.Minutes, 0, TimeSpan.Zero);
                    ScheduleEventModel scheduleEventModel;
                    var appointment = doctor.Appointments.FirstOrDefault(a => a.AppointmentDate == currentAppointmentDate
                                                     && a.AppointmentStatus != Infrastructure.Enums.AppointmentStatus.Cancelled);
                    if (appointment != null)
                    {
                        scheduleEventModel = GetShcedule(appointment);
                    }
                    else
                    {
                        var jointAppointment = _jointDoctorService.GetJointAppointment(doctor.Id, currentAppointmentDate);
                        if (jointAppointment != null)
                        {

                        }
                        scheduleEventModel = GetShcedule(jointAppointment);
                    }
                    scheduleEventModel.Starting = currentAppointmentDate;
                    



                    doctorSchedule.Events.Add(scheduleEventModel);
                }
                doctorSchedules.Add(doctorSchedule);
            }
            return Ok(doctorSchedules);

        }

        private static ScheduleEventModel GetShcedule(Appointment appointment)
        {
            ScheduleEventModel scheduleEventModel = new ScheduleEventModel();
            if (appointment != null)
            {
                scheduleEventModel.Description = "Вы не сможете записаться на это время.";
                scheduleEventModel.IsBusy = true;
                scheduleEventModel.Name = $"{appointment.Patient.LastName} {appointment.Patient.FirstName} {appointment.Patient.Patronymic}";
                scheduleEventModel.AppointmentId = appointment.Id;
                scheduleEventModel.AppointmentStatus = appointment.AppointmentStatus;


                //var currentAppointmentDateLast = new DateTimeOffset(appointment.AppointmentDateLast.Year, appointment.AppointmentDateLast.Month, appointment.AppointmentDateLast.Day, appointment.AppointmentDateLast.Hour, appointment.AppointmentDateLast.Minute, 0, TimeSpan.Zero);
                //scheduleEventModel.Last = currentAppointmentDateLast.ToUniversalTime();
                DateTimeOffset dateTimeOffset1 = appointment.AppointmentDateLast.ToUniversalTime();
                scheduleEventModel.Last = dateTimeOffset1;
            }
            else
            {
                scheduleEventModel.Description = "Вы можете записаться на это время.";
                scheduleEventModel.Name = "Свободно";
                scheduleEventModel.IsBusy = false;
                scheduleEventModel.AppointmentId = 0;
                scheduleEventModel.AppointmentStatus = Infrastructure.Enums.AppointmentStatus.NA;
            }
            return scheduleEventModel;
        }
    }
}
