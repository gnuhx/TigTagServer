using ApplicationCore.Entities.LogAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.LogService;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LogService : ILogService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        public LogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Log> AddAsync(Log entity)
        {
            await _unitOfWork.LogRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public LogicResult<Log> CreateLog(Log log)
        {
            try
            {
                if (log is null)
                {
                    return new LogicResult<Log>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Annotation can not be empty." }
                    };
                }
                _unitOfWork.LogRepository.AddAsync(log);
                _unitOfWork.SaveChangesAsync();
                return new LogicResult<Log>
                {
                    IsSuccess = true,
                    Data = log
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<Log>
                {
                    IsSuccess = false,
                    Errors = new List<string>
                    {
                        "Unable to save changes.",
                        ex.Message
                    }
                };
            }
        }

        public Task<Log> EagerGetByIdAsync(int id)
        {
            return _unitOfWork.LogRepository.EagerGetByIdAsync(id);
        }
        public async Task<Log> GetByIdAsync(int id)
        {
            return await _unitOfWork.LogRepository.GetByIdAsync(id);
        }
        public async Task<List<Log>> GetAllAsync()
        {
            var logs = await _unitOfWork.LogRepository.GetAllAsync();
            return logs;
        }

        public Task<List<Log>> GetByIdsAsync(List<int> ids)
        {
            var result = _unitOfWork.LogRepository.GetQueryable(filter: x => ids.Contains(x.Id)).ToList();
            return Task.FromResult(result);
        }
    }
}
