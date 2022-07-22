using ProductManagement.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.ASP.Models.ClientVM
{
    public class ClientCreateViewModel
    {
        [Required(ErrorMessage = "Champs requis")]
        [MaxLength(50, ErrorMessage = "Trop long")]
        [MinLength(2, ErrorMessage = "Trop court")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Champs requis")]
        [MaxLength(50, ErrorMessage = "Trop long")]
        [MinLength(2, ErrorMessage = "Trop court")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Champs requis")]
        [EmailAddress(ErrorMessage = "Le format n'est pas correct")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Champs requis")]
        public int SelectedGender { get; set; }
    }
}
