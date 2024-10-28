using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BikeLostAndFound.ViewModels
{
    public class CreateAdViewModel
    {
        public int Id { get; set; }

        public string UserID { get; set; }
        
        [Display(Name = "Brand")]
        [Required(ErrorMessage = "Brand is dfd")]
        public string Brand { get; set; }
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Year of Manufacture")]
        public string YearOfManufacture { get; set; }
        [Required]
        [Display(Name = "Engine capacity")]
        public string EngineCapacity { get; set; }
        [Required]
        [Display(Name = "Kilometers run")]
        public string KilometersRun { get; set; }

        [Display(Name = "Owner's Name")]
        public string OwnerName { get; set; }
        [Display(Name = "Owner's Email")]
        [Required]
        [EmailAddress]
        public string OwnerEmail { get; set; }
        [Display(Name = "Owner's Phone No")]
        public string OwnerPhone { get; set; }
        [Display(Name = "Owner's Address")]
        public string OwnerAddress { get; set; }
        [Display(Name = "Bike's Photo")]
        public IFormFile BikePhoto { get; set; }
        [Display(Name = "Bike Registration No")]
        public string BikeRegNo { get; set; }
    }
}
