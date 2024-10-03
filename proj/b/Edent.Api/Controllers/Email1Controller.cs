using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;


namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Email1Controller : Controller
    {

        [HttpPost]
        public IActionResult SendEmail([FromQuery]string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("harhil46@gmail.com"));
            email.To.Add(MailboxAddress.Parse("gg084316@gmail.com"));
            email.Subject = "request.Subject";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 25, SecureSocketOptions.StartTls);
            smtp.Authenticate("harhil46@gmail.com", "fgug josl iozo ijlz");
            smtp.Send(email);
            smtp.Disconnect(true);
            return Ok();
        }
    }
}
