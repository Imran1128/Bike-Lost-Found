using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BikeLostAndFound.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string PhotoPath { get; set; }
        
    }
}
