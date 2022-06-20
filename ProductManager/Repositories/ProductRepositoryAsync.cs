using ProductManager.Data;
using ProductManager.Domain.Entities;
using ProductManager.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ProductManager.Repositories
{
    public class ProductRepositoryAsync : GenericRepositoryAsync<Product>, IProductRepositoryAsync
    {
        public ProductRepositoryAsync(ProductDBContext dbContext) : base(dbContext) { }

        public async Task<ICollection<Product>> GetMostViewedProductsAsync(int count)
        {
            return await this._productDBContext.Product.Include(p => p.Analytics).Where(o => o.IsDeleted == false && o.Analytics.ViewCount >= 1).OrderByDescending(o => o.Analytics.ViewCount).Take(count).ToListAsync();
        }
    }
}
