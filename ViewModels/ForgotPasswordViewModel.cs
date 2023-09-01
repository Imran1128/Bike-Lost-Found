using System.ComponentModel.DataAnnotations;

namespace BikeLostAndFound.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
