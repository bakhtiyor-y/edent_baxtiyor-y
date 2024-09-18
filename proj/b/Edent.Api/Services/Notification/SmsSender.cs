using Edent.Api.Models;
using Edent.Api.Services.Notification.Interfaces;
using System.Threading.Tasks;

namespace Edent.Api.Services.Notification
{
    public class SmsSender<TSender> : ISmsSender<TSender>
    {
        private readonly ISendSmsBehavior<TSender> _sendSmsBehavior;

        public SmsSender(ISendSmsBehavior<TSender> sendSmsBehavior)
        {
            _sendSmsBehavior = sendSmsBehavior;
        }


        public Task<bool> SendAsync(NotificationMessage notification)
        {
            return _sendSmsBehavior.SendAsync(notification);
        }

    }
}
