using System.Collections.Generic;

namespace Edent.Api.Models.Report
{
    public class DentalServiceReport
    {
        public DentalServiceReport()
        {
            Items = new List<DentalServiceReportItem>();
        }
        public IList<DentalServiceReportItem> Items { get; set; }
    }
}
