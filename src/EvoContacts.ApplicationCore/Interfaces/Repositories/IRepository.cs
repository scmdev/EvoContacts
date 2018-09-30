using EvoContacts.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EvoContacts.ApplicationCore.Interfaces
{
    public interface IRepository<T> where T : BaseEntityDeletable
    {
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> criteria);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);

        Task<List<T>> ListAllAsync();

        Task<List<T>> ListAsync(Expression<Func<T, bool>> criteria);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);

        Task<IPager<T>> GetPagedListAsync(int page, int pageSize);
        Task<IPager<T>> GetPagedListAsync(Expression<Func<T, bool>> criteria, int page, int pageSize);
        Task<IPager<T>> GetPagedListAsync(Expression<Func<T, bool>> criteria, int page, int pageSize, params Expression<Func<T, object>>[] includes);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);

        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id, Guid? deletedUserId = null);

    }
}
