using System.ComponentModel.DataAnnotations;

namespace ProductManagement.ASP.Models.OrderVM
{
    public class OrderLineCreateViewModel
    {
        [Required]
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
