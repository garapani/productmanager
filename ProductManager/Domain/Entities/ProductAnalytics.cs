using ProductManager.Domain.Common;

namespace ProductManager.Domain.Entities
{
    public class ProductAnalytics
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ViewCount { get; set; }
    }
}
