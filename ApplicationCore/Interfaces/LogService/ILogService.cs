using ApplicationCore.Entities.LogAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.LogService
{
    public interface ILogService
    {
        Task<Log> EagerGetByIdAsync(int id);
        //Task<bool> UpdateLastTimeStatusAsync(Log log);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<Log> Validate(Log log);
        LogicResult<Log> CreateLog(Log log);
        Task<List<Log>> GetAllAsync();
        Task<List<Log>> GetByIdsAsync(List<int> ids);
        Task<Log> GetByIdAsync(int id);
        Task<Log> AddAsync(Log entity);
    }
}
