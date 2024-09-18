namespace Edent.Api.Models
{
    public class SmtpProvider
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; } = 587; // default smtp port
        public bool UseSSL { get; set; } = true;
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
