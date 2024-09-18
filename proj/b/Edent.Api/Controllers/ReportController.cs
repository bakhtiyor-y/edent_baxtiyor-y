using Edent.Api.Infrastructure.Filters;
using Edent.Api.Models.Report;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IDoctorService _doctorService;
        private readonly IUserResolverService _userResolverService;
        private readonly IPartnerService _partnerService;
        private readonly IDentalServiceService _dentalService;
        private readonly IPatientService _patientService;

        public ReportController(IInvoiceService invoiceService,
            IDoctorService doctorService,
            IUserResolverService userResolverService,
            IPartnerService partnerService,
            IDentalServiceService dentalService,
            IPatientService patientService)
        {
            _invoiceService = invoiceService;
            _doctorService = doctorService;
            _userResolverService = userResolverService;
            _partnerService = partnerService;
            _dentalService = dentalService;
            _patientService = patientService;
        }

        [HttpGet("GetReceptReport")]
        public IActionResult GetReceptReport([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceptReportFilter>(filter);
            var query = _invoiceService.Query()
                .Include(i => i.Recept.Patient)
                .Include("Recept.Employee")
                .Include("Recept.Appointment.Partner")
                .Include("Recept.Doctor.DoctorInTerms.Term")
                .Include("Recept.ReceptInventories.Inventory.InventoryPrices")
                .Where(w => w.CreatedDate.LocalDateTime.Date >= filterModel.From.LocalDateTime.Date && w.CreatedDate.LocalDateTime.Date <= filterModel.To.LocalDateTime.Date)
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
            return Ok(report);
        }

        [HttpGet("GetDoctorsReport")]
        public IActionResult GetDoctorsReport([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DoctorReportFilter>(filter);
            var query = _doctorService.Query()
                .Include(i => i.Recepts.Where(w => w.CreatedDate.LocalDateTime.Date >= filterModel.From.LocalDateTime.Date && w.CreatedDate.LocalDateTime.Date <= filterModel.To.LocalDateTime.Date))
           .Include("Recepts.Invoice")
           .Include("DoctorInTerms.Term")
           .OrderBy(o => o.LastName)
           .ToList();

            DoctorReport doctorReport = new DoctorReport();
            foreach (var item in query)
            {
                DoctorReportItem reportItem = new DoctorReportItem();
                reportItem.Id = item.Id;
                reportItem.FullName = $"{item.LastName} {item.FirstName} {item.Patronymic}";
                reportItem.ReceptCount = item.Recepts.Count;
                reportItem.ClinicIncome = item.Recepts.Sum(s => s.Invoice.TotalSumm);

                var doctorInterm = item.DoctorInTerms.FirstOrDefault();
                if (doctorInterm != null && doctorInterm.Term != null)
                {
                    if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Fixed)
                    {
                        reportItem.Fee = doctorInterm.TermValue * item.Recepts.Count;
                        reportItem.CalculatedSumm = item.Recepts.Where(w => w.IsDoctorCalculated).Count() * doctorInterm.TermValue;
                        reportItem.LeftSumm = item.Recepts.Where(w => !w.IsDoctorCalculated).Count() * doctorInterm.TermValue;
                    }
                    else if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Percent)
                    {
                        reportItem.Fee = item.Recepts.Sum(s => ((s.Invoice.TotalSumm * doctorInterm.TermValue / 100) - s.TechnicShare / 2));
                        reportItem.CalculatedSumm = item.Recepts.Where(w => w.IsDoctorCalculated).Sum(s => ((s.Invoice.TotalSumm * doctorInterm.TermValue / 100) - s.TechnicShare / 2));
                        reportItem.LeftSumm = item.Recepts.Where(w => !w.IsDoctorCalculated).Sum(s => ((s.Invoice.TotalSumm * doctorInterm.TermValue / 100) - s.TechnicShare / 2));
                    }
                    else
                    {
                        reportItem.Fee = 0;
                        reportItem.CalculatedSumm = 0;
                        reportItem.LeftSumm = 0;
                    }
                }
                doctorReport.Items.Add(reportItem);

            }
            return Ok(doctorReport);
        }

        [HttpGet("GetDoctorReceptReport")]
        public IActionResult GetDoctorReceptReport([FromQuery] string filter)
        {
            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
            if (userId == 0)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }
            var doctor = _doctorService.Query()
                .Include("DoctorInTerms.Term")
                .FirstOrDefault(f => f.UserId == userId);
            if (doctor == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor not found" } });
            }

            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DoctorReceptReportFilter>(filter);
            var query = _invoiceService.Query()
                .Include(i => i.Recept.Patient)
                .Include(i => i.Recept.Appointment.Partner)
                .Include("Recept.ReceptInventories.Inventory.InventoryPrices")
                .Where(w => w.CreatedDate.LocalDateTime.Date >= filterModel.From.LocalDateTime.Date
                    && w.CreatedDate.LocalDateTime.Date <= filterModel.To.LocalDateTime.Date
                    && w.Recept.DoctorId == doctor.Id)
                .ToList();

            DoctorReceptReport report = new DoctorReceptReport();
            foreach (var item in query)
            {
                DoctorReceptReportItem reportItem = new DoctorReceptReportItem();
                reportItem.Id = item.ReceptId;
                reportItem.PatientName = $"{item.Recept.Patient.LastName} {item.Recept.Patient.FirstName} {item.Recept.Patient.Patronymic}";
                reportItem.ReceptDate = item.Recept.CreatedDate;
                reportItem.Discount = item.Discount;
                reportItem.TotalIncome = item.TotalSumm;
                reportItem.TechnicOutcome = item.Recept.TechnicShare;
                double clinicTechnicOutcome = item.Recept.TechnicShare;

                var doctorInterm = doctor.DoctorInTerms.FirstOrDefault();
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
                reportItem.Calculated = item.Recept.IsDoctorCalculated ? reportItem.DoctorOutcome : 0;
                reportItem.Left = item.Recept.IsDoctorCalculated ? 0 : reportItem.DoctorOutcome;
                if (item.Recept.Appointment.Partner != null)
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
                report.Items.Add(reportItem);
            }

            report.TotalIncome = report.Items.Sum(s => s.TotalIncome);
            report.TotalDiscount = report.Items.Sum(s => s.Discount);
            report.TotalDoctorOutcome = report.Items.Sum(s => s.DoctorOutcome);
            report.TotalTechnicOutcome = report.Items.Sum(s => s.TechnicOutcome);
            report.TotalPartnerShare = report.Items.Sum(s => s.PartnerShare);
            report.TotalCalculated = report.Items.Sum(s => s.Calculated);
            report.TotalLeft = report.Items.Sum(s => s.Left);

            return Ok(report);
        }

        [HttpGet("GetPartnersReport")]
        public IActionResult GetPartnersReport([FromQuery] string filter)
        {

            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DoctorReportFilter>(filter);
            var query = _partnerService
                .Query()
                .Include("Appointments.Recept.Invoice")
                .Where(w => w.Appointments.Any(a => a.Recept != null && a.Recept.CreatedDate.LocalDateTime.Date <= filterModel.To.LocalDateTime.Date && a.CreatedDate.LocalDateTime.Date >= filterModel.From.LocalDateTime.Date))
                .ToList();

            var report = new PartnersReport();

            foreach (var item in query)
            {
                var reportItem = new PartnersReportItem();
                reportItem.Id = item.Id;
                reportItem.PartnerName = item.Name;
                reportItem.IncomeSumm = item.Appointments.Sum(s => (s.Recept != null && s.Recept.Invoice != null) ? s.Recept.Invoice.TotalSumm : 0);
                if (item.ProfitType == Infrastructure.Enums.ProfitType.Fixed)
                {
                    reportItem.PartnerSumm = item.Profit * item.Appointments.Count(s => s.Recept != null);
                    reportItem.CalculatedSumm = item.Appointments.Count(s => s.Recept != null && s.Recept.IsPartnerCalculated) * item.Profit;
                    reportItem.LeftSumm = item.Appointments.Count(s => s.Recept != null && !s.Recept.IsPartnerCalculated) * item.Profit;
                }
                else
                {
                    reportItem.PartnerSumm = item.Appointments.Sum(s => (s.Recept != null && s.Recept.Invoice != null) ? s.Recept.Invoice.TotalSumm * item.Profit / 100 : 0);
                    reportItem.CalculatedSumm = item.Appointments.Where(w => w.Recept != null && w.Recept.Invoice != null && w.Recept.IsPartnerCalculated).Sum(s => s.Recept.Invoice.TotalSumm * item.Profit / 100);
                    reportItem.LeftSumm = item.Appointments.Where(w => w.Recept != null && w.Recept.Invoice != null && !w.Recept.IsPartnerCalculated).Sum(s => s.Recept.Invoice.TotalSumm * item.Profit / 100);
                }

                report.Items.Add(reportItem);
            }
            return Ok(report);
        }

        [HttpGet("GetDentalServiceReport")]
        public IActionResult GetDentalServiceReport([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DoctorReportFilter>(filter);
            var query = _dentalService
                .Query()
                .Include(i => i.ReceptDentalServices.Where(w => w.CreatedDate.LocalDateTime.Date >= filterModel.From.LocalDateTime.Date && w.CreatedDate.LocalDateTime.Date <= filterModel.To.LocalDateTime.Date))
                .Include(i => i.TreatmentDentalServices.Where(w => w.CreatedDate.LocalDateTime.Date >= filterModel.From.LocalDateTime.Date && w.CreatedDate.LocalDateTime.Date <= filterModel.To.LocalDateTime.Date))
                .Include(i => i.DentalServicePrices)
                .ToList();

            var report = new DentalServiceReport();
            foreach (var item in query)
            {
                var reportItem = new DentalServiceReportItem();
                reportItem.Id = item.Id;
                reportItem.Name = item.Name;
                reportItem.ProvidedCount = item.TreatmentDentalServices.Count + item.ReceptDentalServices.Count;
                var price = item.DentalServicePrices.OrderByDescending(o => o.DateFrom).FirstOrDefault(f => f.DateFrom.LocalDateTime.Date <= filterModel.From.LocalDateTime.Date);
                if (price != null)
                {
                    reportItem.TotalSumm = price.Price * reportItem.ProvidedCount;
                }
                report.Items.Add(reportItem);
            }
            report.Items = report.Items.OrderByDescending(o => o.TotalSumm).ToList();
            return Ok(report);
        }

        [HttpGet("GetPatientsReport")]
        public IActionResult GetPatientsReport([FromQuery] string filter, [FromQuery] string name = null)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _patientService.Query()
                .Include("Appointments.Recept.Invoice");

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lname = name.ToLower();
                query = query.Where(w => w.FirstName.ToLower().Contains(lname) || w.LastName.ToLower().Contains(lname));
            }

            query = query
                .OrderByDescending(o => o.Appointments.Sum(s => s.Recept.Invoice.Debt))
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = query.Select(s => new PatientReportItem
            {
                FullName = $"{s.LastName} {s.FirstName} {s.Patronymic}",
                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                BirthDate = s.BirthDate,
                Balance = s.Balance,
                Debt = s.Appointments.Sum(s => s.Recept.Invoice.Debt),
                Id = s.Id
            }).AsEnumerable();

            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpPost("ReceptChartData")]
        public IActionResult ReceptChartData([FromBody] PeriodFilter filter)
        {

            var query = _invoiceService.Query()
                .Include(i => i.Recept.Patient)
                .Include("Recept.Appointment.Partner")
                .Include("Recept.Doctor.DoctorInTerms.Term")
                .Include("Recept.ReceptInventories.Inventory.InventoryPrices")
                .Where(w => w.CreatedDate.LocalDateTime.Date >= filter.From.LocalDateTime.Date && w.CreatedDate.LocalDateTime.Date <= filter.To.LocalDateTime.Date)
                .ToList();

            CharDataModel<double> charDataModel = new();
            ChartDataSetModel<double> income = new() { Label = "Приход" };
            ChartDataSetModel<double> profit = new() { Label = "Прибыл" };

            if (filter.From.LocalDateTime.Month == filter.To.LocalDateTime.Month && filter.From.LocalDateTime.Year == filter.To.LocalDateTime.Year)
            {
                int length = filter.To.LocalDateTime.Day - filter.From.LocalDateTime.Day;
                for (int i = 0; i <= length; i++)
                {
                    var date = filter.From.LocalDateTime.AddDays(i);
                    charDataModel.Labels.Add(date.Date.ToShortDateString());
                    var incomeValue = query.Where(w => w.CreatedDate.LocalDateTime.Date.Day == date.Date.Day).Sum(s => s.ProvidedSumm) / 1000000;
                    income.Data.Add(Math.Round(incomeValue, 2));
                    var profitValue = query.Where(w => w.CreatedDate.LocalDateTime.Date.Day == date.Date.Day).Sum(s =>
                    {
                        double clinicTechnicOutcome = s.Recept.TechnicShare;
                        var doctorInterm = s.Recept.Doctor.DoctorInTerms.FirstOrDefault();
                        double doctorOutcome = 0;
                        if (doctorInterm != null && doctorInterm.Term != null)
                        {
                            if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Fixed)
                            {
                                doctorOutcome = doctorInterm.TermValue;
                            }
                            else if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Percent)
                            {
                                clinicTechnicOutcome = s.Recept.TechnicShare / 2;
                                doctorOutcome = (s.TotalSumm * doctorInterm.TermValue / 100) - clinicTechnicOutcome;
                            }
                        }
                        double clinicOutcome = s.Recept.ReceptInventories.Sum(s => s.Quantity * s.Inventory.InventoryPrices.FirstOrDefault(f => f.DateFrom.Date <= s.Recept.CreatedDate.Date).Price) + clinicTechnicOutcome;
                        double partnerShare = 0;
                        if (s.Recept.Appointment.PartnerId != null)
                        {
                            if (s.Recept.Appointment.Partner.ProfitType == Infrastructure.Enums.ProfitType.Percent)
                            {
                                partnerShare = s.TotalSumm * s.Recept.Appointment.Partner.Profit / 100;
                            }
                            else
                            {
                                partnerShare = s.Recept.Appointment.Partner.Profit;
                            }
                        }
                        double profit = s.TotalSumm - clinicOutcome - doctorOutcome - partnerShare;
                        return profit;
                    }) / 1000000;
                    profit.Data.Add(Math.Round(profitValue, 2));
                }
                charDataModel.Datasets.Add(income);
                charDataModel.Datasets.Add(profit);
            }
            else
            {
                int length = filter.To.LocalDateTime.Month - filter.From.LocalDateTime.Month;
                for (int i = 0; i <= length; i++)
                {
                    var date = filter.From.LocalDateTime.AddMonths(i);
                    charDataModel.Labels.Add(date.Date.ToString("MMMM"));
                    var incomeValue = query.Where(w => w.CreatedDate.LocalDateTime.Date.Month == date.Date.Month).Sum(s => s.ProvidedSumm) / 1000000;
                    income.Data.Add(Math.Round(incomeValue, 2));
                    var profitValue = query.Where(w => w.CreatedDate.LocalDateTime.Date.Month == date.Date.Month).Sum(s =>
                    {
                        double clinicTechnicOutcome = s.Recept.TechnicShare;
                        var doctorInterm = s.Recept.Doctor?.DoctorInTerms.FirstOrDefault();
                        double doctorOutcome = 0;
                        if (doctorInterm != null && doctorInterm.Term != null)
                        {
                            if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Fixed)
                            {
                                doctorOutcome = doctorInterm.TermValue;
                            }
                            else if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Percent)
                            {
                                clinicTechnicOutcome = s.Recept.TechnicShare / 2;
                                doctorOutcome = (s.TotalSumm * doctorInterm.TermValue / 100) - clinicTechnicOutcome;
                            }
                        }
                        double clinicOutcome = s.Recept.ReceptInventories.Sum(s => s.Quantity * s.Inventory.InventoryPrices.FirstOrDefault(f => f.DateFrom.Date <= s.Recept.CreatedDate.Date).Price) + clinicTechnicOutcome;
                        double partnerShare = 0;
                        if (s.Recept.Appointment.PartnerId != null)
                        {
                            if (s.Recept.Appointment.Partner.ProfitType == Infrastructure.Enums.ProfitType.Percent)
                            {
                                partnerShare = s.TotalSumm * s.Recept.Appointment.Partner.Profit / 100;
                            }
                            else
                            {
                                partnerShare = s.Recept.Appointment.Partner.Profit;
                            }
                        }
                        double profit = s.TotalSumm - clinicOutcome - doctorOutcome - partnerShare;
                        return profit;
                    }) / 1000000;
                    profit.Data.Add(Math.Round(profitValue, 2));
                }
                charDataModel.Datasets.Add(income);
                charDataModel.Datasets.Add(profit);

            }
            return Ok(charDataModel);
        }

        [HttpPost("DoctorChartData")]
        public IActionResult DoctorChartData([FromBody] PeriodFilter filter)
        {
            var query = _doctorService.Query()
                .Include(i => i.Recepts.Where(w => w.CreatedDate.LocalDateTime.Date >= filter.From.LocalDateTime.Date && w.CreatedDate.LocalDateTime.Date <= filter.To.LocalDateTime.Date))
           .Include("Recepts.Invoice")
           .Include("DoctorInTerms.Term")
           .OrderBy(o => o.LastName)
           .ToList();

            CharDataModel<double> charDataModel = new();
            ChartDataSetModel<double> income = new() { Label = "Приход" };
            ChartDataSetModel<double> profit = new() { Label = "Зарплата" };

            foreach (var item in query)
            {
                var firstName = item.FirstName[0].ToString();
                var patronymic = !string.IsNullOrWhiteSpace(item.Patronymic) && item.Patronymic.Length > 1 ? item.Patronymic[0].ToString() : "";
                charDataModel.Labels.Add($"{item.LastName} {firstName}. {(string.IsNullOrWhiteSpace(patronymic) ? "" : $"{patronymic}.")}");
                var incomeValue = item.Recepts.Sum(s => s.Invoice.ProvidedSumm) / 1000000;
                income.Data.Add(Math.Round(incomeValue, 2));
                var doctorInterm = item.DoctorInTerms.FirstOrDefault();
                double profitValue = 0;
                if (doctorInterm != null && doctorInterm.Term != null)
                {
                    if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Fixed)
                    {
                        profitValue = doctorInterm.TermValue * item.Recepts.Count;
                    }
                    else if (doctorInterm.Term.Type == Infrastructure.Enums.TermType.Percent)
                    {
                        profitValue = item.Recepts.Sum(s => ((s.Invoice.TotalSumm * doctorInterm.TermValue / 100) - s.TechnicShare / 2));
                    }
                    else
                    {
                        profitValue = 0;
                    }
                }
                profit.Data.Add(Math.Round(profitValue / 1000000, 2));
            }

            charDataModel.Datasets.Add(income);
            charDataModel.Datasets.Add(profit);

            return Ok(charDataModel);
        }

        [HttpPost("PartnerChartData")]
        public IActionResult PartnerChartData([FromBody] PeriodFilter filter)
        {
            var query = _partnerService
                .Query()
                .Include("Appointments.Recept.Invoice")
                .Where(w => w.Appointments.Any(a => a.Recept.CreatedDate.LocalDateTime.Date <= filter.To.LocalDateTime.Date && a.CreatedDate.LocalDateTime.Date >= filter.From.LocalDateTime.Date))
                .ToList();

            CharDataModel<double> charDataModel = new();
            ChartDataSetModel<double> income = new() { Label = "Приход" };
            ChartDataSetModel<double> profit = new() { Label = "Зарплата" };

            if (filter.From.LocalDateTime.Month == filter.To.LocalDateTime.Month && filter.From.LocalDateTime.Year == filter.To.LocalDateTime.Year)
            {
                int length = filter.To.LocalDateTime.Day - filter.From.LocalDateTime.Day;
                for (int i = 0; i <= length; i++)
                {
                    var date = filter.From.LocalDateTime.AddDays(i);
                    charDataModel.Labels.Add(date.Date.ToShortDateString());
                    var incomeValue = query.Sum(w =>
                        w.Appointments.Where(a => !(a.Recept == null || a.Recept.Invoice == null) && a.Recept.Invoice.CreatedDate.LocalDateTime.Date.Day == date.Date.Day)
                                      .Sum(s => s.Recept.Invoice.ProvidedSumm)) / 1000000;

                    income.Data.Add(Math.Round(incomeValue, 2));
                    var profitValue = query.Sum(w =>
                        w.Appointments.Where(a => !(a.Recept == null || a.Recept.Invoice == null) && a.Recept.Invoice.CreatedDate.LocalDateTime.Date.Day == date.Date.Day)
                                      .Sum(s =>
                                            {
                                                double partnerSum = 0;
                                                if (s.Partner.ProfitType == Infrastructure.Enums.ProfitType.Fixed)
                                                {
                                                    partnerSum = s.Partner.Profit;
                                                }
                                                else
                                                {
                                                    partnerSum = s.Recept.Invoice.TotalSumm * s.Partner.Profit / 100;
                                                }
                                                return partnerSum;
                                            })) / 1000000;
                    profit.Data.Add(Math.Round(profitValue, 2));
                }
                charDataModel.Datasets.Add(income);
                charDataModel.Datasets.Add(profit);
            }
            else
            {
                int length = filter.To.LocalDateTime.Month - filter.From.LocalDateTime.Month;
                for (int i = 0; i <= length; i++)
                {
                    var date = filter.From.LocalDateTime.AddMonths(i);
                    charDataModel.Labels.Add(date.Date.ToString("MMMM"));

                    var incomeValue = query.Sum(w =>
                       w.Appointments.Where(a => !(a.Recept == null || a.Recept.Invoice == null) && a.Recept.Invoice.CreatedDate.LocalDateTime.Date.Month == date.Date.Month)
                                     .Sum(s => s.Recept.Invoice.ProvidedSumm)) / 1000000;

                    income.Data.Add(Math.Round(incomeValue, 2));
                    var profitValue = query.Sum(w =>
                        w.Appointments.Where(a => !(a.Recept == null || a.Recept.Invoice == null) && a.Recept.Invoice.CreatedDate.LocalDateTime.Date.Month == date.Date.Month)
                                      .Sum(s =>
                                      {
                                          double partnerSum = 0;
                                          if (s.Partner.ProfitType == Infrastructure.Enums.ProfitType.Fixed)
                                          {
                                              partnerSum = s.Partner.Profit;
                                          }
                                          else
                                          {
                                              partnerSum = (s.Recept.Invoice.TotalSumm * s.Partner.Profit / 100);
                                          }
                                          return partnerSum;
                                      })) / 1000000;
                    profit.Data.Add(Math.Round(profitValue, 2));
                }
                charDataModel.Datasets.Add(income);
                charDataModel.Datasets.Add(profit);

            }

            return Ok(charDataModel);
        }

        [HttpPost("PatientChartData")]
        public IActionResult PatientChartData([FromBody] PeriodFilter filter)
        {
            var query = _patientService.Query()
                .Include("Appointments.Recept.Invoice")
                .Where(w => w.CreatedDate.LocalDateTime <= filter.To.LocalDateTime.Date && w.CreatedDate.LocalDateTime.Date >= filter.From.LocalDateTime.Date)
                .ToList();

            CharDataModel<double> charDataModel = new();
            ChartDataSetModel<double> common = new() { Label = "Значение" };
            var debtValue = query.Sum(w => w.Appointments.Sum(s => (s.Recept == null || s.Recept.Invoice == null) ? 0 : s.Recept.Invoice.Debt)) / 1000000;
            common.Data.Add(Math.Round(debtValue, 2));
            var creditVal = query.Sum(w => w.Balance) / 1000000;
            common.Data.Add(Math.Round(creditVal, 2));
            var serviceVal = query.Sum(w => w.Appointments.Sum(s => (s.Recept == null || s.Recept.Invoice == null) ? 0 : s.Recept.Invoice.ProvidedSumm)) / 1000000;
            common.Data.Add(Math.Round(serviceVal, 2));

            charDataModel.Labels.Add("Долги");
            charDataModel.Labels.Add("Предоплата");
            charDataModel.Labels.Add("Оказанные услуги");
            charDataModel.Datasets.Add(common);

            return Ok(charDataModel);
        }

        [HttpPost("ServiceChartData")]
        public IActionResult ServiceChartData([FromBody] PeriodFilter filter)
        {

            var query = _dentalService
                .Query()
                .Include(i => i.ReceptDentalServices.Where(w => w.CreatedDate.LocalDateTime.Date >= filter.From.LocalDateTime.Date && w.CreatedDate.LocalDateTime.Date <= filter.To.LocalDateTime.Date))
                .Include(i => i.TreatmentDentalServices.Where(w => w.CreatedDate.LocalDateTime.Date >= filter.From.LocalDateTime.Date && w.CreatedDate.LocalDateTime.Date <= filter.To.LocalDateTime.Date))
                .Include(i => i.DentalServicePrices)
                .ToList();

            var reportItems = new List<DentalServiceReportItem>();
            foreach (var item in query)
            {
                var reportItem = new DentalServiceReportItem();
                reportItem.Id = item.Id;
                reportItem.Name = item.Name;
                reportItem.ProvidedCount = item.TreatmentDentalServices.Count + item.ReceptDentalServices.Count;
                var price = item.DentalServicePrices.OrderByDescending(o => o.DateFrom.LocalDateTime).FirstOrDefault(f => f.DateFrom.LocalDateTime.Date <= filter.To.LocalDateTime.Date);
                if (price != null)
                {
                    reportItem.TotalSumm = price.Price * reportItem.ProvidedCount;
                }
                if (reportItem.TotalSumm == 0)
                {
                    continue;
                }
                reportItems.Add(reportItem);
            }
            CharDataModel<double> charDataModel = new();
            ChartDataSetModel<double> dataSetModel = new() { Label = "Значение" };


            double totalSum = reportItems.Sum(s => s.TotalSumm);
            var commonServices = reportItems.Where(w => w.TotalSumm > (totalSum * 0.17)).ToList();
            foreach (var item in commonServices)
            {
                charDataModel.Labels.Add(item.Name);
                var sumVal = item.TotalSumm / 1000000;
                dataSetModel.Data.Add(Math.Round(sumVal, 2));
            }
            charDataModel.Labels.Add("Другие");
            double otherSum = reportItems.Where(w => w.TotalSumm < (totalSum * 0.17)).Sum(s => s.TotalSumm);
            var otherSumVal = otherSum / 1000000;
            dataSetModel.Data.Add(Math.Round(otherSumVal, 2));

            charDataModel.Datasets.Add(dataSetModel);
            return Ok(charDataModel);
        }

        [HttpGet("GetPartnerReceptReport/{partnerId}")]
        public IActionResult GetPartnerReceptReport(int partnerId, [FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceptReportFilter>(filter);
            var query = _invoiceService.Query()
                .Include(i => i.Recept.Patient)
                .Include("Recept.Employee")
                .Include("Recept.Appointment.Partner")
                .Include("Recept.Doctor.DoctorInTerms.Term")
                .Include("Recept.ReceptInventories.Inventory.InventoryPrices")
                .Where(w => w.Recept != null
                            && w.Recept.Appointment != null
                            && w.Recept.Appointment.PartnerId == partnerId
                            && w.CreatedDate.LocalDateTime.Date >= filterModel.From.LocalDateTime.Date
                            && w.CreatedDate.LocalDateTime.Date <= filterModel.To.LocalDateTime.Date)
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
            return Ok(report);
        }
    }
}
