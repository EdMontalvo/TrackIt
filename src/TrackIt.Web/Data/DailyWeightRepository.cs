using System;
using System.Diagnostics;
using System.Linq;

namespace TrackIt.Web.Data
{
    public class DailyWeightRepository : IDailyWeightRepository
    {
        private readonly DailyWeightContext _ctx;

        public DailyWeightRepository(DailyWeightContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<DailyWeight> GetWeights()
        {
            return _ctx.DailyWeights;
        }

        public IQueryable<DailyWeight> GetWeightsByUser(string userName)
        {
            return _ctx.DailyWeights.Where(d => d.UserName == userName);
        }

        public bool Save()
        {

            try
            {
                return _ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                //TODO add logging
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool AddDailyWeight(DailyWeight newWeight)
        {
            try
            {
                var weight =
                    _ctx.DailyWeights.SingleOrDefault(
                        d => d.UserName == newWeight.UserName && d.WeightDate == newWeight.WeightDate);
                if (weight == null)
                {
                    _ctx.DailyWeights.Add(newWeight);
                }
                else
                {
                    newWeight.DailyWeightId = weight.DailyWeightId;
                    newWeight.BodyFat = weight.BodyFat;
                    weight.Weight = newWeight.Weight;
                }
                return true;
            }
            catch (Exception ex)
            {
                //TODO add logging
                return false;
            }
        }
    }
}