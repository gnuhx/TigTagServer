using ApplicationCore.Entities.NotificationAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.NotificationsService
{
    public interface INotificationService
    {
        Task<Notification> EagerGetByIdAsync(int id);
        Task<LogicResult<Notification>> CreateNotification(Notification annotation);
        Task<List<Notification>> GetAllAsync();
        Task<List<Notification>> GetByIdsAsync(List<int> ids);
        Task<Notification> UpdateAsync(Notification annotation);
        //Task<bool> UpdateLastTimeStatusAsync(Annotation annotation);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<Annotation> Validate(Annotation annotation);
        Task<Notification> GetByIdAsync(int id);
        Task<Notification> AddAsync(Notification entity);
        Task<List<Notification>> GetByUserIdAsync(int id);
        Task<List<int>> GetAllIdsAsync();
        Task<Notification> DeleteAsync(Notification entity);
        Task<Notification> GetExpiredByUserIdAsync(int id);
    }
}
