using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Models.Report
{
    public class DentalServiceReportFilter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Id { get; set; }
    }
}
