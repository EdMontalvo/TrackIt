using System.Linq;

namespace TrackIt.Web.Data
{
    public interface IDailyWeightRepository
    {

        IQueryable<DailyWeight> GetWeights();

        IQueryable<DailyWeight> GetWeightsByUser(string userName);

        bool Save();

        bool AddDailyWeight(DailyWeight newWeight);

    }
}