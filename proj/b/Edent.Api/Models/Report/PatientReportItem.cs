using System;
namespace Edent.Api.Models.Report
{
    public class PatientReportItem
    {
        public PatientReportItem()
        {
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public double Balance { get; set; }
        public double Debt { get; set; }
        public string Email { get; set; }
    }
}
