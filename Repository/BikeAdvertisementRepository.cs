using System;
using BikeLostAndFound.Data;
using BikeLostAndFound.Interfaces;
using BikeLostAndFound.Models;
using CodeFirst.Repositories.Base;
using System.Linq;

namespace BikeLostAndFound.Repository
{
    public class BikeAdvertisementRepository : BaseRepository<BikeAdvertisement>, IBikeAdvertisement
    {
        private readonly MyDbContext myDbContext;

        public BikeAdvertisementRepository(MyDbContext myDbContext) : base(myDbContext)
        {
            this.myDbContext = myDbContext;
        }
    }
}
