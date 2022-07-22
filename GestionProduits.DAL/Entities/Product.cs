using ProductManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Column(TypeName = "char")]
        [StringLength(8)] 
        public string Reference { get; set; }

        [Required, MinLength(4), MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int Stock { get; set; }

        [Required, Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime UpdateDate { get; set; }

        public bool Delete { get; set; }
        public ICollection<Category> Categories { get; set; }

    }
}
