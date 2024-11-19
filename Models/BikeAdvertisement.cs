using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BikeLostAndFound.Models
{
    public class BikeAdvertisement
    {
       
            public int Id { get; set; }
            [NotMapped]
            public string EncryptedId { get; set; }
            public string UserID { get; set; }
        [Required]
        [Display(Name = "Brand")]
            public string Brand { get; set; }
        [Required]
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
        [Required]
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
            public string BikePhoto { get; set; }
        [Display(Name = "Bike Registration No")]
        public string BikeRegNo { get; set; }
        [Required]
        [Display(Name = "Engine Number")]
        public string EngineNumber { get; set; }
        [Required]
        [Display(Name = "Chassis Number")]
        public string ChassisNumber { get; set; }
        [Required]
        [Display(Name = "Price")]
        public string Price { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }



    }
    }



