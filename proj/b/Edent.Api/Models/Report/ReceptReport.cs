using System.Collections.Generic;

namespace Edent.Api.Models.Report
{
    public class ReceptReport
    {
        public ReceptReport()
        {
            Items = new List<ReceptReportItem>();
        }
        public IList<ReceptReportItem> Items { get; set; }
        public double TotalClinicIncome { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalIncome { get; set; }
        public double TotalClinicOutcome { get; set; }
        public double TotalDoctorOutcome { get; set; }
        public double TotalTechnicOutcome { get; set; }
        public double TotalPartnerShare { get; set; }
        public double TotalProfit { get; set; }
    }
}
