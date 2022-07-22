using ProductManagement.DAL.Entities;

namespace ProductManagement.ASP.Models.ClientVM
{
    public class ClientIndexViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Reference { get; set; }
    }
}
