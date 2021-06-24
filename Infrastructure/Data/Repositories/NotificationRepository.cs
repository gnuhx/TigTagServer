using ApplicationCore.Entities.NotificationAggregate;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class NotificationRepository : EfRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(NoisContext context) : base(context)
        {

        }

        public async Task<Notification> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Notification>> GetNotificationByUserId(int userId)
        {
            var preRes =  await DbSet.Where(x => x.UserId == userId).ToListAsync();
            var result = preRes.Where(x=>x.IsChecked ==false).ToList();
            return result;
        }
    }
}
