using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Repositories.Identity;

namespace ApplicationCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        Task CreateTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();
        #region Repositories
        IRefreshTokenRepository RefreshTokenRepository { get; }
        ILogRepository LogRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IRelationRepository RelationRepository { get; }
        #endregion
    }
}
