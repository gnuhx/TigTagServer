using ApplicationCore.Entities.RelationAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IRelationRepository : IRepositoryAsync<Relation>
    {
        //Task<Relation> EagerGetByIdAsync(int id);
    }
}
