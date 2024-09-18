using System;

namespace Edent.Api.Models.Report
{
    public class DoctorReceptReportItem
    {
        public int Id { get; set; }
        public DateTimeOffset ReceptDate { get; set; }
        public string PatientName { get; set; }
        public double Discount { get; set; }
        public double TotalIncome { get; set; }
        public double TechnicOutcome { get; set; }
        public double PartnerShare { get; set; }
        public double DoctorOutcome { get; set; }
        public double Calculated { get; set; }
        public double Left { get; set; }

    }
}
