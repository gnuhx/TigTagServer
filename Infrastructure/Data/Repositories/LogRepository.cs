using ApplicationCore.Entities.LogAggregate;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class LogRepository : EfRepository<Log>, ILogRepository
    {
        public LogRepository(NoisContext context) : base(context)
        {

        }
        public async Task<Log> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}
