using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.Models;
using Edent.Api.Models.Report;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System;
using System.Linq;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReceptController : ControllerBase
    {
        private readonly IReceptService _receptService;
        private readonly IUserResolverService _userResolverService;
        private readonly IAppointmentService _appointmentService;
        private readonly IInvoiceService _invoiceService;
        private readonly IDentalServiceReceptInventorySettingService _dentalServiceReceptInventorySettingService;
        private readonly IInventoryOutcomeService _inventoryOutcomeService;
        private readonly IInventoryService _inventoryService;
        private readonly IDoctorService _doctorService;

        public ReceptController(IReceptService receptService,
            IUserResolverService userResolverService,
            IAppointmentService appointmentService,
            IInvoiceService invoiceService,
            IDentalServiceReceptInventorySettingService dentalServiceReceptInventorySettingService,
            IInventoryOutcomeService inventoryOutcomeService,
            IInventoryService inventoryService,
            IDoctorService doctorService)
        {
            _receptService = receptService;
            _userResolverService = userResolverService;
            _appointmentService = appointmentService;
            _invoiceService = invoiceService;
            _dentalServiceReceptInventorySettingService = dentalServiceReceptInventorySettingService;
            _inventoryOutcomeService = inventoryOutcomeService;
            _inventoryService = inventoryService;
            _doctorService = doctorService;
        }

        [HttpGet("GetByDoctor")]
        public IActionResult GetByDoctor([FromQuery] string filter, [FromQuery] string name = null)
        {
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

            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _receptService
                .Query()
                .Include(i => i.Patient)
                .Where(w => w.DoctorId == doctor.Id);

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lname = name.ToLower();
                query = query.Where(w => w.Patient.FirstName.ToLower().Contains(lname) || w.Patient.LastName.ToLower().Contains(lname));
            }

            var resultQuery = query.OrderByDescending(o => o.CreatedDate)
                .PrimengTableFilter(filterModel, out int totalRecord)
                .AsEnumerable();

            return Ok(new { Data = resultQuery, Total = totalRecord });
        }

        [HttpPost]
        public IActionResult Post(ReceptViewModel viewModel)
        {
            var recept = _receptService.Mapper.Map<Recept>(viewModel);

            var dentalServiceIds = viewModel.Treatments.SelectMany(a => a.TreatmentDentalServices.Select(s => s.DentalServiceId));
            var rptDentalServiceIds = viewModel.ReceptDentalServices.Select(a => a.DentalServiceId);

            var dentalServiceReceptInventories = _dentalServiceReceptInventorySettingService
                .Query()
                .Include(i => i.ReceptInventorySetting)
                    .ThenInclude(th => th.ReceptInventorySettingItems)
                .Where(w => dentalServiceIds.Contains(w.DentalServiceId) || rptDentalServiceIds.Contains(w.DentalServiceId))
                .ToList();

            var items = dentalServiceReceptInventories.SelectMany(s => s.ReceptInventorySetting.ReceptInventorySettingItems);
            foreach (var item in items)
            {
                var receptInventory = new ReceptInventory
                {
                    InventoryId = item.InventoryId,
                    MeasurementUnitId = item.MeasurementUnitId,
                    Quantity = item.Quantity
                };
                recept.ReceptInventories.Add(receptInventory);
            }

            var created = _receptService.Create(recept);

            var appointment = _appointmentService.GetById(created.AppointmentId);

            if (appointment != null)
            {
                appointment.AppointmentStatus = Infrastructure.Enums.AppointmentStatus.CarriedOut;
                _appointmentService.Repository.Edit(appointment);
                _appointmentService.Repository.SaveChanges();
            }

            _invoiceService.ProvideInvoice(created.Id);

            if (_inventoryOutcomeService.ProvideReceptOutcome(created.Id))
            {
                _inventoryService.UpdateInventoriesStock(created.Id);
            }

            return Ok(recept);
        }


        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _receptService
                .Query()
                .AsNoTracking()
                .Include(i => i.Patient)
                .Include("ReceptDentalServices.DentalService.DentalServicePrices")
                .Include("Treatments.PatientTooth.Tooth")
                .Include("Treatments.TreatmentDentalServices.DentalService.DentalServicePrices")
                .Include("Treatments.Diagnose")
                .FirstOrDefault(w => w.Id == id);

            foreach (var item in result.ReceptDentalServices)
            {
                item.DentalService.DentalServicePrices = item.DentalService.DentalServicePrices.Where(w => w.DateFrom < result.CreatedDate).OrderByDescending(o => o.DateFrom).ToList();
            }

            foreach (var item in result.Treatments)
            {
                foreach (var td in item.TreatmentDentalServices)
                {
                    td.DentalService.DentalServicePrices = td.DentalService.DentalServicePrices.Where(w => w.DateFrom < result.CreatedDate).OrderByDescending(o => o.DateFrom).ToList();
                }
            }

            return Ok(result);
        }

        [HttpPut("CalculateWithDoctor")]
        public IActionResult CalculateWithDoctor(ReceptCalculateDoctorViewModel model)
        {
            var recept = _receptService
                            .Query()
                            .AsNoTracking()
                            .FirstOrDefault(w => w.Id == model.Id);

            recept.IsDoctorCalculated = model.IsDoctorCalculated;
            _receptService.Repository.Edit(recept);

            return Ok(new ApiResultModel<bool> { Result = _receptService.Repository.SaveChanges() });
        }

        [HttpPut("CalculateWithTechnic")]
        public IActionResult CalculateWithTechnic(ReceptCalculateTechnicViewModel model)
        {
            var recept = _receptService
                            .Query()
                            .AsNoTracking()
                            .FirstOrDefault(w => w.Id == model.Id);

            recept.IsTechnicCalculated = model.IsTechnicCalculated;
            _receptService.Repository.Edit(recept);

            return Ok(new ApiResultModel<bool> { Result = _receptService.Repository.SaveChanges() });
        }

        [HttpPut("CalculateWithPartner")]
        public IActionResult CalculateWithPartner(ReceptCalculatePartnerViewModel model)
        {
            var recept = _receptService
                             .Query()
                             .AsNoTracking()
                             .FirstOrDefault(w => w.Id == model.Id);

            recept.IsPartnerCalculated = model.IsPartnerCalculated;
            _receptService.Repository.Edit(recept);

            return Ok(new ApiResultModel<bool> { Result = _receptService.Repository.SaveChanges() });
        }

        [HttpPut("SetTechnic")]
        public IActionResult SetTechnic(SetTechnicViewModel model)
        {
            var recept = _receptService
                             .Query()
                             .AsNoTracking()
                             .FirstOrDefault(w => w.Id == model.Id);

            recept.TechnicId = model.TechnicId;
            recept.TechnicShare = model.TechnicShare;

            _receptService.Repository.Edit(recept);

            return Ok(new ApiResultModel<bool> { Result = _receptService.Repository.SaveChanges() });
        }

        [HttpGet("GetPatientRecepts")]
        public IActionResult GetPatientRecepts(int patientId)
        {
            var recepts = _receptService
                             .Query()
                             .Include("ReceptDentalServices.DentalService.DentalServicePrices")
                             .Include("Treatments.PatientTooth.Tooth")
                             .Include("Treatments.TreatmentDentalServices.DentalService.DentalServicePrices")
                             .Include("Treatments.Diagnose")
                             .AsNoTracking()
                             .Where(w => w.PatientId == patientId)
                             .AsEnumerable();

            foreach (var recept in recepts)
            {
                foreach (var item in recept.ReceptDentalServices)
                {
                    item.DentalService.DentalServicePrices = item.DentalService.DentalServicePrices.Where(w => w.DateFrom < recept.CreatedDate).OrderByDescending(o => o.DateFrom).ToList();
                }

                foreach (var item in recept.Treatments)
                {
                    foreach (var td in item.TreatmentDentalServices)
                    {
                        td.DentalService.DentalServicePrices = td.DentalService.DentalServicePrices.Where(w => w.DateFrom < recept.CreatedDate).OrderByDescending(o => o.DateFrom).ToList();
                    }
                }
            }


            return Ok(recepts);
        }

        [HttpGet("GetByAppointment/{id}")]
        public IActionResult GetByAppointment(int id)
        {
            var result = _receptService
                .Query()
                .Include(i => i.Patient)
                .Include("ReceptDentalServices.DentalService.DentalServicePrices")
                .Include("Treatments.PatientTooth.Tooth")
                .Include("Treatments.TreatmentDentalServices.DentalService.DentalServicePrices")
                .Include("Treatments.Diagnose")
                .FirstOrDefault(w => w.AppointmentId == id);


            foreach (var item in result.ReceptDentalServices)
            {
                item.DentalService.DentalServicePrices = item.DentalService.DentalServicePrices.Where(w => w.DateFrom < result.CreatedDate).OrderByDescending(o => o.DateFrom).ToList();
            }

            foreach (var item in result.Treatments)
            {
                foreach (var td in item.TreatmentDentalServices)
                {
                    td.DentalService.DentalServicePrices = td.DentalService.DentalServicePrices.Where(w => w.DateFrom < result.CreatedDate).OrderByDescending(o => o.DateFrom).ToList();
                }
            }

            return Ok(result);
        }

        [HttpGet("GetByPartner/{id}")]
        public IActionResult GetByPartner(int id, [FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DoctorReportFilter>(filter);
            var result = _receptService
                .Query()
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.Employee)
                .Include(i => i.Appointment)
                .Where(w => w.Appointment.PartnerId == id
                        && w.CreatedDate.LocalDateTime.Date <= filterModel.To.LocalDateTime.Date
                        && w.CreatedDate.LocalDateTime.Date >= filterModel.From.LocalDateTime.Date);

            return Ok(result);
        }

    }
}
