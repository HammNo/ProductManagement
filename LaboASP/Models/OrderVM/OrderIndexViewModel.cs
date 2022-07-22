using ProductManagement.DAL.Entities;

namespace ProductManagement.ASP.Models.OrderVM
{
    public class OrderIndexViewModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
        public string ClientName { get; set; }
    }
}
