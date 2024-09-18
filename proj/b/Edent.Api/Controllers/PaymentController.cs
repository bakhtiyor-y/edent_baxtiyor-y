using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Filters;
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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IInvoiceService _invoiceService;

        public PaymentController(IPaymentService paymentService, IInvoiceService invoiceService)
        {
            _paymentService = paymentService;
            _invoiceService = invoiceService;
        }


        [HttpPost("ProvidePayment")]
        public IActionResult ProvidePayment(PaymentViewModel payment)
        {
            var invoice = _invoiceService
                .Query()
                .Include("Recept.Patient")
                .FirstOrDefault(f => f.Id == payment.InvoiceId);

            if (invoice != null && invoice.Recept != null && invoice.Recept.Patient != null)
            {
                if (invoice.Debt == 0)
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "You cannot provide payment, because invoice is closed" } });
                }

                if (payment.IsFromBalance && invoice.Recept.Patient.Balance < payment.PayedSumm)
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "In patient balance insufficient funds" } });
                }

                var entity = new Payment
                {
                    InvoiceId = payment.InvoiceId,
                    PayedSumm = payment.PayedSumm,
                    PaymentType = payment.PaymentType,
                    IsFromBalance = payment.IsFromBalance
                };

                var createdPayment = _paymentService.Create(entity);
                if (createdPayment.Id > 0)
                {
                    if (createdPayment.IsFromBalance)
                    {
                        invoice.Recept.Patient.Balance = invoice.Recept.Patient.Balance - createdPayment.PayedSumm;
                    }

                    if (invoice.Debt - payment.PayedSumm < 0)
                    {
                        invoice.Recept.Patient.Balance = invoice.Recept.Patient.Balance + (payment.PayedSumm - invoice.Debt);
                        invoice.Debt = 0;
                    }
                    else
                    {
                        invoice.Debt = invoice.Debt - payment.PayedSumm;
                    }
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
                }
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on create payment" } });
            }
            return BadRequest(new JsonErrorResponse { Messages = new[] { "Invoice not found" } });
        }

        [HttpGet("GetInvoicePayments")]
        public IActionResult GetInvoicePayments(int invoiceId)
        {
            var invoice = _invoiceService.GetById(invoiceId);
            if (invoice != null)
            {
                var query = _paymentService.Query()
                    .Where(w => w.InvoiceId == invoiceId)
                    .OrderByDescending(o => o.CreatedDate);

                var result = _invoiceService.Mapper.Map<IEnumerable<PaymentViewModel>>(query);
                return Ok(result);

            }
            return BadRequest(new JsonErrorResponse { Messages = new[] { "Invoice not found" } });
        }
    }
}
