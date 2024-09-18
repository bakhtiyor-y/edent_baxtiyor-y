using System;

namespace Edent.Api.ViewModels
{
    public class InvoiceViewModel : IViewModel
    {
        public int Id { get; set; }
        public int ReceptId { get; set; }
        public string PatientFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string DoctorFullName { get; set; }
        public DateTimeOffset ReceptDate { get; set; }
        public double TotalSumm { get; set; }
        public double Discount { get; set; }
        public double ProvidedSumm { get; set; }
        public double Debt { get; set; }
        public double PatientBalance { get; set; }
    }
}
