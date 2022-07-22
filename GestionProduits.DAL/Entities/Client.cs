using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Entities
{
    public class Client
    {
        public int Id { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Column(TypeName = "char")]
        [StringLength(8)]
        public string Reference { get; set; }

        [Required]
        public GenderType Gender { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
