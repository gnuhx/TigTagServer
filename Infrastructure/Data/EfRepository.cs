using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class EfRepository<T> : IRepositoryAsync<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly NoisContext Context;
        protected readonly DbSet<T> DbSet;

        public EfRepository(NoisContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public Task<List<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null)
        {
            return GetQueryable(null, orderBy, includeProperties, countSkip, countTake).ToListAsync();
        }

        #region iqueryable
        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (countSkip.HasValue)
            {
                query = query.Skip(countSkip.Value);
            }

            if (countTake.HasValue)
            {
                query = query.Take(countTake.Value);
            }

            return query;
        }
        #endregion

#pragma warning disable 1998
        public async Task UpdateAsync(T entity)
#pragma warning restore 1998
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

#pragma warning disable 1998
        public async Task DeleteAsync(T entity)
#pragma warning restore 1998
        {
            DbSet.Remove(entity);
        }

        #region BulkExtensions
        public Task BulkInsertAsync(List<T> entities)
        {
            var bulkOptions = new BulkConfig();

            //Check FK
            bulkOptions.SqlBulkCopyOptions = SqlBulkCopyOptions.CheckConstraints;
            //Get returned Ids
            bulkOptions.SetOutputIdentity = true;

            return Context.BulkInsertAsync(entities, bulkConfig: bulkOptions);
        }

        public Task BulkUpdateAsync(List<T> entities)
        {
            return Context.BulkUpdateAsync(entities);
        }

        public Task BulkDeleteAsync(List<T> entities)
        {
            return Context.BulkDeleteAsync(entities);
        }

        #endregion BulkExtensions

    }
}
