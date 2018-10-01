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

        Task<T> GetSingleAsync(Expression<Func<T, bool>> criteria);

        Task<List<T>> ListAllAsync();

        Task<IPager<T>> GetPagedListAsync(int page, int pageSize);

        Task<int> CountAsync();

        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id, Guid? deletedUserId = null);

    }
}
