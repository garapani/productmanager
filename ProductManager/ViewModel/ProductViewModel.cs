using ProductManager.Domain.Constansts;

namespace ProductManager.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public int ViewCount { get; set; }
    }
}