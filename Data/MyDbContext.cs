using BikeLostAndFound.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BikeLostAndFound.Data
{
    public class MyDbContext:IdentityDbContext<ApplicationUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
        {
            
        }
        DbSet<LostAndFoundBikeInformation> lostAndFoundBikeInformation { get; set; }
        DbSet<BikeAdvertisement> bikeAdvertisements { get; set; }
    }
}
