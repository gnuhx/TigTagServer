using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.AzureBlobService;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Interfaces.Identity;
using ApplicationCore.Interfaces.LogService;
using ApplicationCore.Interfaces.NotificationsService;
using ApplicationCore.Interfaces.RabbitMQ;
using ApplicationCore.Interfaces.RelationService;

namespace Infrastructure.Services
{
    public class Service : IService
    {
        public Service(IEmailService emailService,
                       IIdentityService identityService,
                       IRabbitMQService rabbitMQService,
                       IAzureBlobService azureBlobService,
                       ILogService logService,
                       INotificationService notificationService,
                       IRelationService relationService)
        {
            EmailService = emailService;
            IdentityService = identityService;
            RabbitMQService = rabbitMQService;
            AzureBlobService = azureBlobService;
            LogService = logService;
            NotificationService = notificationService;
            RelationService = relationService;
        }

        public IEmailService EmailService { get; }
        public IIdentityService IdentityService { get; }
        public IRabbitMQService RabbitMQService { get; }
        public IAzureBlobService AzureBlobService { get; }
        public ILogService LogService { get; }
        public INotificationService NotificationService { get; }
        public IRelationService RelationService { get; }
    }
}
