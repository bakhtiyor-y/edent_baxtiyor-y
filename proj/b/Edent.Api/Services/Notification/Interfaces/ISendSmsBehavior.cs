using Edent.Api.Models;
using System.Threading.Tasks;

namespace Edent.Api.Services.Notification.Interfaces
{
    public interface ISendSmsBehavior<TSender>
    {
        Task<bool> SendAsync(NotificationMessage message);
    }
}
