using ApplicationCore.Entities.NotificationAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface INotificationRepository : IRepositoryAsync<Notification>
    {
        Task<Notification> EagerGetByIdAsync(int id);
        Task<List<Notification>> GetNotificationByUserId(int userId);
    }
}
