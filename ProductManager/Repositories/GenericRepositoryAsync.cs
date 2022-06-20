using ProductManager.Data;
using ProductManager.Domain.Common;
using ProductManager.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace ProductManager.Repositories
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        protected readonly ProductDBContext _productDBContext;
        public GenericRepositoryAsync(ProductDBContext productDBContext)
        {
            _productDBContext = productDBContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            _productDBContext.Entry(entity).State = EntityState.Added; // added row
            var addedEntity = this._productDBContext.Set<T>().Add(entity);
            //var addedEntity = await _productDBContext.Set<T>().AddAsync(entity);
            await _productDBContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var foundItem = await _productDBContext.Set<T>().FindAsync(id);
            if (foundItem != null)
            {
                var deletedEntry = _productDBContext.Set<T>().Remove(foundItem);
                await _productDBContext.SaveChangesAsync();
                return deletedEntry.Entity;
            }
            else
            {
                throw new System.Exception("failed to find item with id:" + id);
            }
        }
        public virtual async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, string includeProperties = "")
        {
            var query = _productDBContext.Set<T>().Where(expression);
            foreach (var includeProperty in includeProperties.Split
               (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.Where(expression).ToListAsync().ConfigureAwait(false);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _productDBContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _productDBContext.Set<T>().Skip(pageNumber - 1 * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _productDBContext.Entry(entity).State = EntityState.Modified;
            await _productDBContext.SaveChangesAsync();
        }
    }
}
