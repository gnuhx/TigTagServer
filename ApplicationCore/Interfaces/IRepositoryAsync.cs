using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IRepositoryAsync<T> where T : BaseEntity, IAggregateRoot
    {
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<List<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null);
        #region iqueryable
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, object>>[] includeProperties = null, int? countSkip = null, int? countTake = null);
        #endregion

        Task BulkInsertAsync(List<T> entities);
        Task BulkUpdateAsync(List<T> entities);
        Task BulkDeleteAsync(List<T> entities);


    }
}
