using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace BikeLostAndFound.Models
{
    public class ApplicationUser:IdentityUser
    {
        [NotMapped]
        
        public string EncryptedId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string PhotoPath { get; set; }
        public DateTimeOffset LastloginDate { get; set; }


    }
}
