using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Query;

namespace BikeLostAndFound.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required]
        
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
