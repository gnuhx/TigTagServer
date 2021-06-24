using ApplicationCore.Entities.RelationAggregate;
using ApplicationCore.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class RelationRepository : EfRepository<Relation>, IRelationRepository
    {
        public RelationRepository(NoisContext context) : base(context)
        {

        }
    }
}
