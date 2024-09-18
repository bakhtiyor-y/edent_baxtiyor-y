using Edent.Api.Models;
using System.Threading.Tasks;

namespace Edent.Api.Services.Notification.Interfaces
{
    public interface ISendEmailBehavior<TSender>
    {
        Task<bool> SendAsync(NotificationMessage notification);
    }
}
