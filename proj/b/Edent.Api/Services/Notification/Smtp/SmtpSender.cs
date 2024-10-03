using Edent.Api.Models;
using Edent.Api.Services.Notification.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Edent.Api.Services.Notification.Smtp
{
    public class SmtpSender : ISendEmailBehavior<SmtpSender>
    {
        private readonly IOptionsSnapshot<SmtpProvider> _emailProvider;
        private readonly ILogger<SmtpSender> _logger;

        public SmtpSender(IOptionsSnapshot<SmtpProvider> emailProvider,
            ILogger<SmtpSender> logger)
        {
            _emailProvider = emailProvider;
            _logger = logger;
        }

        public Task<bool> SendAsync(NotificationMessage notification)
        {
            try
            {
                if (_emailProvider.Value != null)
                {

                    var client = new SmtpClient
                    {
                        Host = _emailProvider.Value.Host,
                        Port = _emailProvider.Value.Port,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = true,
                        Credentials = new System.Net.NetworkCredential(_emailProvider.Value.UserName, _emailProvider.Value.Password)
                    };

                    var message = getEmailMessage(notification, _emailProvider.Value.UserName);
                    client.Send(message);

                    return Task.FromResult(true);
                }
                else
                {
                    _logger.LogError($"-------- Email sender: Email provider not found");
                    return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"-------- Email sender: {ex.Message} inner exception: {ex.InnerException?.Message}");
                return Task.FromResult(false);
            }

        }
        private MailMessage getEmailMessage(NotificationMessage notification, string from)
        {
            return new MailMessage(from, notification.Recipient, notification.Title, notification.Message);
        }
    }
}
