using BikeLostAndFound.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.Options;
using System.Threading.Tasks;
using System.Security.Claims;
using BikeLostAndFound.Models;

namespace BikeLostAndFound.Data
{
    public class ApplicationUserClaimPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> option) : base(userManager, roleManager, option)
        {


        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var Identity =  await base.GenerateClaimsAsync(user);
            Identity.AddClaim(new Claim("Name", user.Name ?? ""));
            Identity.AddClaim(new Claim("PhotoPath", user.PhotoPath ?? ""));
            Identity.AddClaim(new Claim("Address", user.Address ?? ""));
            Identity.AddClaim(new Claim("Gender", user.Gender ?? ""));
            Identity.AddClaim(new Claim("Email", user.Email ?? ""));
            Identity.AddClaim(new Claim("PhoneNumber", user.PhoneNumber ?? ""));
            Identity.AddClaim(new Claim("Id", user.Id ?? ""));
            return Identity;
        }
    }
}
