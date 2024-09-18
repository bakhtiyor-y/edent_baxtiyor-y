using Edent.Api.Models;
using Edent.Api.Models.Report;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WidgetController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IDoctorService _doctorService;
        private readonly IUserResolverService _userResolverService;
        private readonly IPartnerService _partnerService;
        private readonly IDentalServiceService _dentalService;
        private readonly IPatientService _patientService;
        private readonly IReceptService _receptService;

        public WidgetController(IInvoiceService invoiceService,
            IDoctorService doctorService,
            IUserResolverService userResolverService,
            IPartnerService partnerService,
            IDentalServiceService dentalService,
            IPatientService patientService,
            IReceptService receptService)
        {
            _invoiceService = invoiceService;
            _doctorService = doctorService;
            _userResolverService = userResolverService;
            _partnerService = partnerService;
            _dentalService = dentalService;
            _patientService = patientService;
            _receptService = receptService;
        }

        [HttpGet("NewPatientCount")]
        public IActionResult GetReceptReport([FromQuery] string filter)
        {
            //PeriodFilter filterModel = new PeriodFilter { From = DateTimeOffset.Now.AddDays(-1), To = DateTimeOffset.Now.AddDays(1) };
            //if (!string.IsNullOrWhiteSpace(filter))
            //{
            //    filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<PeriodFilter>(filter);
            //}

            var count = _patientService.Query()
                .Count(w => w.CreatedDate.LocalDateTime.Date == DateTimeOffset.Now.LocalDateTime.Date);

            return Ok(new ApiResultModel<int> { Result = count });
        }

        [HttpGet("ReceptCount")]
        public IActionResult GetDoctorsReport([FromQuery] string filter)
        {
            //PeriodFilter filterModel = new PeriodFilter { From = DateTimeOffset.Now.AddDays(-1), To = DateTimeOffset.Now.AddDays(1) };
            //if (!string.IsNullOrWhiteSpace(filter))
            //{
            //    filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<PeriodFilter>(filter);
            //}

            var count = _receptService.Query()
                .Count(w => w.CreatedDate.LocalDateTime.Date == DateTimeOffset.Now.LocalDateTime.Date);

            return Ok(new ApiResultModel<int> { Result = count });
        }

        [HttpGet("DentalServiceCount")]
        public IActionResult GetDoctorReceptReport([FromQuery] string filter)
        {
            //PeriodFilter filterModel = new PeriodFilter { From = DateTimeOffset.Now.AddDays(-1), To = DateTimeOffset.Now.AddDays(1) };
            //if (!string.IsNullOrWhiteSpace(filter))
            //{
            //    filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<PeriodFilter>(filter);
            //}
            var result = _dentalService.Query()
                .Include(i => i.TreatmentDentalServices.Where(w => w.CreatedDate.Date == DateTimeOffset.Now.LocalDateTime.Date))
                .ToList();

            var count = result.Count(w => w.TreatmentDentalServices.Count > 0);

            return Ok(new ApiResultModel<int> { Result = count });
        }

        [HttpGet("Incomes")]
        public IActionResult GetPartnersReport([FromQuery] string filter)
        {

            //PeriodFilter filterModel = new PeriodFilter { From = DateTimeOffset.Now.AddDays(-1), To = DateTimeOffset.Now.AddDays(1) };
            //if (!string.IsNullOrWhiteSpace(filter))
            //{
            //    filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<PeriodFilter>(filter);
            //}
            var summ = _invoiceService.Query()
                .Where(w => w.CreatedDate.LocalDateTime.Date == DateTimeOffset.Now.LocalDateTime.Date)
                .Sum(s => s.TotalSumm);


            return Ok(new ApiResultModel<double> { Result = summ });
        }

        [HttpGet("Profits")]
        public IActionResult GetDentalServiceReport([FromQuery] string filter)
        {
            //PeriodFilter filterModel = new PeriodFilter { From = DateTimeOffset.Now.AddDays(-1), To = DateTimeOffset.Now.AddDays(1) };
            //if (!string.IsNullOrWhiteSpace(filter))
            //{
            //    filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<PeriodFilter>(filter);
            //}
            var query = _invoiceService.Query()
                .Include(i => i.Recept.Patient)
                .Include("Recept.Appointment.Partner")
                .Include("Recept.Employee")
                .Include("Recept.Doctor.DoctorInTerms.Term")
                .Include("Recept.ReceptInventories.Inventory.InventoryPrices")
                .Where(w => w.CreatedDate.LocalDateTime.Date == DateTimeOffset.Now.LocalDateTime.Date)
                .ToList();

            ReceptReport report = new ReceptReport();
            foreach (var item in query)
            {
                ReceptReportItem reportItem = new ReceptReportItem();
                reportItem.Id = item.ReceptId;
                if (item.Recept.Doctor != null)
                {
                    reportItem.DoctorName = $"{item.Recept.Doctor.LastName} {item.Recept.Doctor.FirstName} {item.Recept.Doctor.Patronymic}";
                }
                else if (item.Recept.Employee != null)
                {
                    reportItem.DoctorName = $"{item.Recept.Employee.LastName} {item.Recept.Employee.FirstName} {item.Recept.Employee.Patronymic}";
                }
                reportItem.PatientName = $"{item.Recept.Patient.LastName} {item.Recept.Patient.FirstName} {item.Recept.Patient.Patronymic}";
                reportItem.ReceptDate = item.Recept.CreatedDate;
                reportItem.ClinicIncome = item.ProvidedSumm;
                reportItem.Discount = item.Discount;
                reportItem.TotalIncome = item.TotalSumm;
                reportItem.IsDoctorCalculated = item.Recept.IsDoctorCalculated;
                reportItem.IsPartnerCalculated = item.Recept.IsPartnerCalculated;
                reportItem.IsTechnicCalculated = item.Recept.IsTechnicCalculated;
                reportItem.TechnicOutcome = item.Recept.TechnicShare;
                double clinicTechnicOutcome = item.Recept.TechnicShare;

                var doctorInterm = item.Recept.Doctor?.DoctorInTerms.FirstOrDefault();
                if (doctorInterm != null && doctorInterm.Term != null)
                {
                    if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Fixed)
                    {
                        reportItem.DoctorOutcome = doctorInterm.TermValue;
                    }
                    else if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Percent)
                    {
                        clinicTechnicOutcome = item.Recept.TechnicShare / 2;
                        reportItem.DoctorOutcome = (item.TotalSumm * doctorInterm.TermValue / 100) - clinicTechnicOutcome;
                    }
                    else
                    {
                        reportItem.DoctorOutcome = 0;
                    }
                }
                reportItem.ClinicOutcome = item.Recept.ReceptInventories.Sum(s => s.Quantity * s.Inventory.InventoryPrices.FirstOrDefault(f => f.DateFrom.Date <= item.Recept.CreatedDate.Date).Price) + clinicTechnicOutcome;
                if (item.Recept.Appointment.PartnerId != null)
                {
                    if (item.Recept.Appointment.Partner.ProfitType == Infrastructure.Enums.ProfitType.Percent)
                    {
                        reportItem.PartnerShare = item.TotalSumm * item.Recept.Appointment.Partner.Profit / 100;
                    }
                    else
                    {
                        reportItem.PartnerShare = item.Recept.Appointment.Partner.Profit;
                    }
                }
                else
                {
                    reportItem.PartnerShare = 0;
                }
                reportItem.Profit = reportItem.TotalIncome - reportItem.ClinicOutcome - reportItem.DoctorOutcome - reportItem.PartnerShare;
                report.Items.Add(reportItem);
            }

            report.TotalClinicIncome = report.Items.Sum(s => s.ClinicIncome);
            report.TotalClinicOutcome = report.Items.Sum(s => s.ClinicOutcome);
            report.TotalIncome = report.Items.Sum(s => s.TotalIncome);
            report.TotalDiscount = report.Items.Sum(s => s.Discount);
            report.TotalDoctorOutcome = report.Items.Sum(s => s.DoctorOutcome);
            report.TotalTechnicOutcome = report.Items.Sum(s => s.TechnicOutcome);
            report.TotalPartnerShare = report.Items.Sum(s => s.PartnerShare);
            report.TotalProfit = report.Items.Sum(s => s.Profit);
            return Ok(new ApiResultModel<double> { Result = report.TotalProfit });
        }


    }
}
