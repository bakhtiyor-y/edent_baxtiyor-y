using Edent.Api.Controllers.BaseControllers;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Controllers
{
    public class DentalChairController : BaseApiController<DentalChairViewModel, DentalChair>
    {
        private readonly IDentalChairService _dentalChairService;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserResolverService _userResolverService;
        private readonly IDoctorService _doctorService;
        private readonly IDoctorDentalChairService _doctorDentalChairService;

        public DentalChairController(IDentalChairService dentalChairService,
            IAppointmentService appointmentService,
            IUserResolverService userResolverService,
            IDoctorService doctorService,
            IDoctorDentalChairService doctorDentalChairService) : base(dentalChairService)
        {
            _dentalChairService = dentalChairService;
            _appointmentService = appointmentService;
            _userResolverService = userResolverService;
            _doctorService = doctorService;
            _doctorDentalChairService = doctorDentalChairService;
        }

        [HttpGet("GetAll")]
        public virtual IActionResult GetAll()
        {
            var result = _dentalChairService.GetAll<DentalChairViewModel>();
            return Ok(result);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            if (name == null)
                name = string.Empty;

            var query = _dentalChairService
                .Query()
                .Where(w => w.Name.ToLower().StartsWith(name.ToLower()))
                .OrderBy(o => o.Id)
                .ToList();

            var result = _entityService.Mapper.Map<IEnumerable<DentalChairViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<DentalChairViewModel> models)
        {
            foreach (var item in models)
            {
                var dentalChair = _dentalChairService.Mapper.Map<DentalChair>(item);
                _dentalChairService.Repository.Remove(dentalChair);
            }
            return Ok(_dentalChairService.Repository.SaveChanges());
        }

        [HttpGet("GetDentalChairsByDate")]
        public virtual IActionResult GetByDate(long date)
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

            var chairs = _doctorDentalChairService
                .Query()
                .Include(i => i.DentalChair)
                .Where(w => w.DoctorId == doctor.Id)
                .Select(s => s.DentalChair)
                .ToList();

            if (chairs == null || chairs.Count == 0)
            {
                chairs = _dentalChairService.GetAll().ToList();
            }

            var appointments = _appointmentService.Query()
                .Where(w => appointmentDate.Date == w.AppointmentDate.Date)
                .ToList();

            List<AppointmentDentalChairViewModel> result = new();

            foreach (var item in chairs)
            {
                var viewModel = new AppointmentDentalChairViewModel();
                viewModel.Id = item.Id;
                viewModel.Name = item.Name;
                viewModel.IsBusy = appointments.Any(a => a.AppointmentDate == appointmentDate);
                viewModel.Description = viewModel.IsBusy ? "Занято" : "Свободно";
                result.Add(viewModel);
            }

            return Ok(result);
        }

        [HttpGet("GetDoctorDentalChairsByDate")]
        public virtual IActionResult GetByDate(int doctorId, long date)
        {
            var appointmentDate = DateTimeOffset.FromUnixTimeMilliseconds(date);

            var chairs = _doctorDentalChairService
                .Query()
                .Include(i => i.DentalChair)
                .Where(w => w.DoctorId == doctorId)
                .Select(s => s.DentalChair)
                .ToList();

            if (chairs == null || chairs.Count == 0)
            {
                chairs = _dentalChairService.GetAll().ToList();
            }

            var appointments = _appointmentService.Query()
                .Where(w => appointmentDate.Date == w.AppointmentDate.Date)
                .ToList();

            List<AppointmentDentalChairViewModel> result = new();

            foreach (var item in chairs)
            {
                var viewModel = new AppointmentDentalChairViewModel();
                viewModel.Id = item.Id;
                viewModel.Name = item.Name;
                viewModel.IsBusy = appointments.Any(a => a.AppointmentDate == appointmentDate);
                viewModel.Description = viewModel.IsBusy ? "Занято" : "Свободно";
                result.Add(viewModel);
            }

            return Ok(result);
        }
    }
}
