using ProductManagement.DAL.Entities;

namespace ProductManagement.ASP.Models.ClientVM
{
    public class ClientDetailsViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mail { get; set; }

        public string Reference { get; set; }

        public GenderType Gender { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
