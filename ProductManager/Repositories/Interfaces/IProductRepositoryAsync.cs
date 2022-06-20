using ProductManager.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManager.Repositories.Interfaces
{
    public interface IProductRepositoryAsync: IGenericRepositoryAsync<Product>
    {
        public Task<ICollection<Product>> GetMostViewedProductsAsync(int count);
    }
}
