
using System.ComponentModel.DataAnnotations;

namespace BikeLostAndFound.Models
{
    public class LostAndFoundBikeInformation
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Bike Name")]
        public string BikeName { get; set; }
        [Display(Name = "Bike Registration No")]
        public string BikeRegNo { get; set;}
        [Display(Name = "Bike Sn")]
        public string BikeSN { get; set; }
        [Required]
        [Display(Name = "Place Where Lost")]
        public string PlaceWhereLost { get; set; }
        public string Date { get; set; }
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
        public string BikePhoto { get; set;}
        [Display(Name ="GD Number")]
        public string GDNumber { get; set; }
        public bool IsFound { get; set; }

        

    }
}
