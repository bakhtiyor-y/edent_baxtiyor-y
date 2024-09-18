using Edent.Api.Models;
using System.Threading.Tasks;

namespace Edent.Api.Services.Notification.Interfaces
{
    public interface ISmsSender<TSender>
    {
        Task<bool> SendAsync(NotificationMessage notification);
    }
}
