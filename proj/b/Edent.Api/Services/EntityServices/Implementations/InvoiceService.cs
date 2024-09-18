using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class InvoiceService : EntityService<Invoice>, IInvoiceService
    {
        private readonly IReceptService _receptService;

        public InvoiceService(IRepository<Invoice> repository, IMapper mapper, IReceptService receptService)
            : base(repository, mapper)
        {
            _receptService = receptService;
        }

        public Invoice ProvideInvoice(int receptId)
        {
            var recept = _receptService
                .Query()
                .Include("ReceptDentalServices.DentalService.DentalServicePrices")
                .Include("Treatments.TreatmentDentalServices.DentalService.DentalServicePrices")
                .FirstOrDefault(f => f.Id == receptId);

            var invoice = new Invoice
            {
                Discount = 0,
                DiscountType = Infrastructure.Enums.DiscountType.Percent
            };

            double providedSumm = 0;
            foreach (var treatment in recept.Treatments)
            {
                foreach (var dentalService in treatment.TreatmentDentalServices)
                {
                    var lastPrice = dentalService?.DentalService?.DentalServicePrices?.OrderByDescending(o => o.DateFrom).FirstOrDefault();
                    if (lastPrice != null)
                    {
                        providedSumm += lastPrice.Price;
                    }
                }
            }
            foreach (var dentalService in recept.ReceptDentalServices)
            {
                var lastPrice = dentalService?.DentalService?.DentalServicePrices?.OrderByDescending(o => o.DateFrom).FirstOrDefault();
                if (lastPrice != null)
                {
                    providedSumm += lastPrice.Price;
                }
            }
            invoice.ReceptId = recept.Id;
            invoice.ProvidedSumm = providedSumm;
            invoice.TotalSumm = providedSumm - invoice.Discount;
            invoice.Debt = providedSumm - invoice.Discount;

            Repository.Add(invoice);
            Repository.SaveChanges();
            return invoice;
        }
    }
}
