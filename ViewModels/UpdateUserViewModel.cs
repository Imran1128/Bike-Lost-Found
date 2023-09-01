using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BikeLostAndFound.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]

        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Gender { get; set; }
        
        public IFormFile Photo { get; set; }
        public string ExistingPhoto { get; set; }
    }
}
