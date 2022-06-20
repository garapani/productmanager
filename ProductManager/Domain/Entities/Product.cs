using ProductManager.Domain.Common;
using ProductManager.Domain.Constansts;

namespace ProductManager.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public virtual ProductAnalytics Analytics { get; set; }
    }
}
