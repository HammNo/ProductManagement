using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Column(TypeName = "char")]
        [StringLength(10)]
        public string Reference { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public int ClientId { get; set; }
        [Required]
        public Client Client { get; set; }
        public ICollection<OrderLine>? OrderLines { get; set; }
    }
}
