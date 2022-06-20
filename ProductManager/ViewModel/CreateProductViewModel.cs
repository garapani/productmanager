using System.ComponentModel.DataAnnotations;

namespace ProductManager.ViewModel
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage ="Product name required")]        
        public string Name { get; set; }

        [Required(ErrorMessage ="Product price required")]
        [Range(1, double.MaxValue, ErrorMessage ="Price should be greater than 1")]
        public double Price { get; set; }
        
        public string Description { get; set; }
    }
}
