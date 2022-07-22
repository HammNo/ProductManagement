using System.ComponentModel.DataAnnotations;

namespace ProductManagement.ASP.Models.CategoryVM
{
    public class CategoryEditViewModel
    {
        public int Id { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
