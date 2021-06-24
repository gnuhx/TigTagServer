using ApplicationCore.Entities.RelationAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.RelationService
{
    public interface IRelationService
    {
        Task<Relation> EagerGetByIdAsync(int id);
        Task<LogicResult<Relation>> CreateNotification(Relation entity);
        Task<Relation> UpdateAsync(Relation entity);
        Task<Relation> GetByIdAsync(int id);
        Task<Relation> AddAsync(Relation entity);
        Task<List<Relation>> GetByUserIdAsync(int id);
        Task<Relation> DeleteAsync(Relation entity);
        Task<List<Relation>> GetAllPaging(int pageIndex);
    }
}
