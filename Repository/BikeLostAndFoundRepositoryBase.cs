using BikeLostAndFound.Data;
using BikeLostAndFound.Interfaces;
using BikeLostAndFound.Models;
using CodeFirst.Repositories.Base;
using System.Linq;

namespace BikeLostAndFound.Repository
{
    public class BikeLostAndFoundRepositoryBase:BaseRepository<LostAndFoundBikeInformation>,IBikeLostAndFoundRepository
    {
        private readonly MyDbContext myDbContext;

        public BikeLostAndFoundRepositoryBase(MyDbContext myDbContext) : base(myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        public LostAndFoundBikeInformation GetByReg(string BikeRegNo)
        {
           return myDbContext.Set<LostAndFoundBikeInformation>().FirstOrDefault(e=> e.BikeRegNo == BikeRegNo);
        }
    }
}
