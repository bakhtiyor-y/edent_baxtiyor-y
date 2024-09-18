using Edent.Api.Infrastructure.Enums;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentService _paymentService;

        public InvoiceController(IInvoiceService invoiceService, IPaymentService paymentService)
        {
            _invoiceService = invoiceService;
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string filter, [FromQuery] string name = null)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _invoiceService
                .Query()
                .Include("Recept.Doctor")
                .Include("Recept.Employee")
                .Include("Recept.Patient");

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lname = name.ToLower();
                query = query.Where(w => w.Recept.Patient.FirstName.ToLower().StartsWith(lname) || w.Recept.Patient.LastName.ToLower().StartsWith(lname));
            }

            var resultQuery = query.OrderByDescending(o => o.CreatedDate)
                .PrimengTableFilter(filterModel, out int totalRecord)
                .AsEnumerable();

            var result = _invoiceService.Mapper.Map<IEnumerable<InvoiceViewModel>>(resultQuery);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpGet("GetByPatient")]
        public IActionResult GetByPatient(int patientId)
        {
           
            var query = _invoiceService
                .Query()
                .Where(w=>w.Recept.PatientId == patientId)
                .Include("Recept.Doctor")
                .Include("Recept.Patient")
                .OrderByDescending(o => o.CreatedDate)
                .AsEnumerable();

            var result = _invoiceService.Mapper.Map<IEnumerable<InvoiceViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("ProvideDiscount")]
        public IActionResult ProvideDiscount(DiscountViewModel discount)
        {
            var invoice = _invoiceService.GetById(discount.InvoiceId);
            if (invoice != null)
            {
                var payments = _paymentService.Query().Count(c => c.InvoiceId == invoice.Id);
                if (payments > 0)
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "One or more payments provided. You can not provide a discount" } });
                }

                invoice.DiscountType = discount.DiscountType;
                if (discount.DiscountType == DiscountType.Percent && discount.Discount > 0 && discount.Discount <= 1)
                {
                    discount.Discount = discount.Discount * 100;
                }
                invoice.Discount = discount.DiscountType == DiscountType.Fixed ? discount.Discount : (invoice.ProvidedSumm * discount.Discount / 100);
                invoice.TotalSumm = invoice.ProvidedSumm - invoice.Discount;
                invoice.Debt = invoice.ProvidedSumm - invoice.Discount;
                _invoiceService.Repository.Edit(invoice);
                if (_invoiceService.Repository.SaveChanges())
                {
                    var updated = _invoiceService
                      .Query()
                      .Include("Recept.Doctor")
                      .Include("Recept.Patient")
                      .FirstOrDefault(f => f.Id == invoice.Id);

                    var result = _invoiceService.Mapper.Map<InvoiceViewModel>(updated);
                    return Ok(result);
                }
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on update invoice" } });
            }
            return BadRequest(new JsonErrorResponse { Messages = new[] { "Invoice not found" } });
        }
    }
}
