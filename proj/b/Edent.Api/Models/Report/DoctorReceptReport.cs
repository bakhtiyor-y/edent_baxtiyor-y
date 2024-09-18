using System.Collections.Generic;

namespace Edent.Api.Models.Report
{
    public class DoctorReceptReport
    {
        public DoctorReceptReport()
        {
            Items = new List<DoctorReceptReportItem>();
        }
        public IList<DoctorReceptReportItem> Items { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalIncome { get; set; }
        public double TotalDoctorOutcome { get; set; }
        public double TotalTechnicOutcome { get; set; }
        public double TotalPartnerShare { get; set; }
        public double TotalCalculated { get; set; }
        public double TotalLeft { get; set; }
    }
}
