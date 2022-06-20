using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductManager.Repositories.Interfaces
{
    public interface IGenericRepositoryAsync<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, string includeProperties = "");
    }
}
