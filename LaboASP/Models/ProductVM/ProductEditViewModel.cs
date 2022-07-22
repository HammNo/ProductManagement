using ProductManagement.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.ASP.Models.ProductVM
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public string? Reference { get; set; }

        [Required(ErrorMessage = "Champs requis")]
        [MaxLength(50, ErrorMessage = "Trop long")]
        [MinLength(4, ErrorMessage = "Trop court")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Champs requis")]
        [MaxLength(1000, ErrorMessage = "Trop long")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Champs requis")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Supérieur ou égal à 0 et max deux décimales")]
        public string Price { get; set; }

        [RegularExpression(@"^-?\d+$", ErrorMessage = "Pas de décimales")]
        public int? StockOffset { get; set; }
        public int Stock { get; set; }

        public List<int>? SelectedCategories { get; set; }
        public List<Category>? AllCategories { get; set; }
    }
}
