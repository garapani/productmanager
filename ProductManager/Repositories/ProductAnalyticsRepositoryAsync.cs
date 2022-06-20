using ProductManager.Data;
using ProductManager.Domain.Entities;
using ProductManager.Repositories.Interfaces;

namespace ProductManager.Repositories
{
    public class ProductAnalyticsRepositoryAsync : GenericRepositoryAsync<ProductAnalytics>, IProductAnalyticsRepositoryAsync
    {
        public ProductAnalyticsRepositoryAsync(ProductDBContext dbContext) : base(dbContext) { }
    }
}
