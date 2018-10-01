using EvoContacts.ApplicationCore.Entities;
using EvoContacts.ApplicationCore.Interfaces;
using EvoContacts.ApplicationCore.Models;
using EvoContacts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EvoContacts.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntityDeletable
    {
        protected readonly EvoContactsDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(EvoContactsDbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            T entity = (await _dbSet.FindAsync(id));

            if (entity != null && entity.IsDeleted)
            {
                entity = null;
            }

            return entity;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> criteria)
        {
            return await _dbSet.Where(x => !x.IsDeleted).FirstOrDefaultAsync(criteria);
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<IPager<T>> GetPagedListAsync(int page, int pageSize)
        {
            var queryableResult = _dbSet.Where(x => !x.IsDeleted);

            PagedList<T> list = await GetPagedListAsync(queryableResult, page, pageSize);

            return list;
        }

        private async Task<PagedList<T>> GetPagedListAsync(IQueryable<T> query, int page, int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var totalRecords = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            var items = await query.Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>()
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

            return pagedList;
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.Where(x => !x.IsDeleted).CountAsync();
        }

        public async Task<bool> AddAsync(T entity)
        {
            entity.CreatedDateTimeOffset = DateTimeOffset.UtcNow;
            _dbContext.Set<T>().Add(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            entity.UpdatedDateTimeOffset = DateTimeOffset.UtcNow;
            _dbContext.Set<T>().Update(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid? deletedUserId = null)
        {
            bool deleted = false;

            T entity = await GetByIdAsync(id);

            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.UpdatedUserId = deletedUserId;
                entity.UpdatedDateTimeOffset = DateTimeOffset.UtcNow;

                _dbContext.Set<T>().Update(entity);

                deleted = await _dbContext.SaveChangesAsync() > 0;
            }

            return deleted;
        }

    }
}
