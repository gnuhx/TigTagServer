using ApplicationCore.Entities.LogAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface ILogRepository : IRepositoryAsync<Log>
    {
        Task<Log> EagerGetByIdAsync(int id);
    }
}
