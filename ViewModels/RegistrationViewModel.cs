using BikeLostAndFound.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BikeLostAndFound.ViewModels
{
    public class RegistrationViewModel
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
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and Confirm Password don't match")]
        public string ConfirmPassword { get; set; }
        public IFormFile  Photo { get; set; }
        public ApplicainUser applicainUser { get; set; }
    }
}
