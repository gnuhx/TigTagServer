using ApplicationCore.Interfaces.AzureBlobService;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Interfaces.Identity;
using ApplicationCore.Interfaces.LogService;
using ApplicationCore.Interfaces.NotificationsService;
using ApplicationCore.Interfaces.RabbitMQ;
using ApplicationCore.Interfaces.RelationService;

namespace ApplicationCore.Interfaces
{
    public interface IService
    {
        IEmailService EmailService { get; }
        IIdentityService IdentityService { get; }
        IRabbitMQService RabbitMQService { get; }
        IAzureBlobService AzureBlobService { get; }
        ILogService LogService { get; }
        INotificationService NotificationService { get; }
        IRelationService RelationService { get; }
    }
}
