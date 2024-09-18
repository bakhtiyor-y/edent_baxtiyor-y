using System;

namespace Edent.Api.Models.Report
{
    public class DoctorReceptReportFilter
    {
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }
}
