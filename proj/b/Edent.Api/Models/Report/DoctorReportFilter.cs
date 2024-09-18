using System;

namespace Edent.Api.Models.Report
{
    public class DoctorReportFilter
    {
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }
}
