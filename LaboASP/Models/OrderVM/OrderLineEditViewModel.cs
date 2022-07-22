using ProductManagement.ASP.Models.ProductVM;

namespace ProductManagement.ASP.Models.OrderVM
{
    public class OrderLineEditViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ProductIndexViewModel Product { get; set; }
    }
}
