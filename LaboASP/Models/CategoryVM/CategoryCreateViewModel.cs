using System.ComponentModel.DataAnnotations;

namespace ProductManagement.ASP.Models.CategoryVM
{
    public class CategoryCreateViewModel
    {
        [Required(ErrorMessage = "Champs requis")]
        [MaxLength(50, ErrorMessage = "Trop long")]
        [MinLength(2, ErrorMessage = "Trop court")]
        public string Name { get; set; }

        [MaxLength(1000, ErrorMessage = "Trop long")]
        public string? Description { get; set; }
    }
}
