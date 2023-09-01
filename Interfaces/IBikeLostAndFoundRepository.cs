using BikeLostAndFound.Data;
using BikeLostAndFound.Models;
using System.Threading.Tasks;

namespace BikeLostAndFound.Interfaces
{
    public interface IBikeLostAndFoundRepository:IBaseService<LostAndFoundBikeInformation>
    {
        
       LostAndFoundBikeInformation GetByReg(string BikeRegNo);

    }
}
