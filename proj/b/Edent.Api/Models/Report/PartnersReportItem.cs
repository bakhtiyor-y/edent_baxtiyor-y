namespace Edent.Api.Models.Report
{
    public class PartnersReportItem
    {
        public int Id { get; set; }
        public string PartnerName { get; set; }
        public double IncomeSumm { get; set; }
        public double PartnerSumm { get; set; }
        public double LeftSumm { get; set; }
        public double CalculatedSumm { get; set; }

    }
}
