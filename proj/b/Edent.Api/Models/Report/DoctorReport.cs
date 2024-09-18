using System.Collections.Generic;

namespace Edent.Api.Models.Report
{
    public class DoctorReport
    {
        public DoctorReport()
        {
            Items = new List<DoctorReportItem>();
        }
        public IList<DoctorReportItem> Items { get; set; }
    }
}
