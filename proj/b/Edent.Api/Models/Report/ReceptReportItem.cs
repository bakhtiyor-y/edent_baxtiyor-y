using System;

namespace Edent.Api.Models.Report
{
    public class ReceptReportItem
    {
        public int Id { get; set; }
        public DateTimeOffset ReceptDate { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public double ClinicIncome { get; set; }
        public double Discount { get; set; }
        public double TotalIncome { get; set; }
        public double ClinicOutcome { get; set; }
        public double DoctorOutcome { get; set; }
        public double TechnicOutcome { get; set; }
        public double PartnerShare { get; set; }
        public double Profit { get; set; }
        public bool IsDoctorCalculated { get; set; }
        public bool IsPartnerCalculated { get; set; }
        public bool IsTechnicCalculated { get; set; }
    }
}
