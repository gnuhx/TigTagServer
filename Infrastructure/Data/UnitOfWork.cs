using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Repositories.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private IDbContextTransaction _transaction;
        public UnitOfWork(NoisContext context,
            IRefreshTokenRepository refreshTokenRepository,
            ILogRepository logRepository,
            INotificationRepository notificationRepository,
            IRelationRepository relationRepository
            )
        {
            Context = context;
            RefreshTokenRepository = refreshTokenRepository;
            LogRepository = logRepository;
            NotificationRepository = notificationRepository;
            RelationRepository = relationRepository;
        }
        public DbContext Context { get; }
        #region Repositories
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public ILogRepository LogRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IRelationRepository RelationRepository { get; }
        #endregion


        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        public async Task CreateTransactionAsync()
        {
            _transaction = await Context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }


        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
