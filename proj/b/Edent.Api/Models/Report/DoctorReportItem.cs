namespace Edent.Api.Models.Report
{
    public class DoctorReportItem
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int ReceptCount { get; set; }
        public double ClinicIncome { get; set; }
        public double Fee { get; set; }
        public double CalculatedSumm { get; set; }
        public double LeftSumm { get; set; }
    }
}
