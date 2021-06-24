using ApplicationCore.Entities.RelationAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.RelationService;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RelationService : IRelationService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        public RelationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Relation> AddAsync(Relation entity)
        {
            throw new NotImplementedException();
        }

        public Task<LogicResult<Relation>> CreateNotification(Relation entity)
        {
            throw new NotImplementedException();
        }

        public Task<Relation> DeleteAsync(Relation entity)
        {
            throw new NotImplementedException();
        }

        public Task<Relation> EagerGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Relation>> GetAllPaging(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public Task<Relation> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Relation>> GetByUserIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Relation> UpdateAsync(Relation entity)
        {
            throw new NotImplementedException();
        }
    }
}
