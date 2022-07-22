using System.ComponentModel.DataAnnotations;

namespace ProductManagement.ASP.Models.OrderVM
{
    public class OrderCreateViewModel
    {
        [Required]
        public int ClientId { get; set; }

        public List<string>? OrderLines { get; set; }
    }
}
