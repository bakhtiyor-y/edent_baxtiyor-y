using Edent.Api.Models;
using Edent.Api.Services.Notification.Interfaces;
using System.Threading.Tasks;

namespace Edent.Api.Services.Notification
{
    public class EmailSender<TSender> : IEmailSender<TSender>
    {
        private readonly ISendEmailBehavior<TSender> _sendEmailBehavior;

        public EmailSender(ISendEmailBehavior<TSender> sendEmailBehavior)
        {
            _sendEmailBehavior = sendEmailBehavior;
        }


        public Task<bool> SendAsync(NotificationMessage notification)
        {
            return _sendEmailBehavior.SendAsync(notification);
        }
    }
}
