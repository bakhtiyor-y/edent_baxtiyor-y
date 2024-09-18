using System.Collections.Generic;

namespace Edent.Api.Models.Report
{
    public class PartnersReport
    {
        public PartnersReport()
        {
            Items = new List<PartnersReportItem>();
        }
        public IList<PartnersReportItem> Items { get; set; }
    }
}
