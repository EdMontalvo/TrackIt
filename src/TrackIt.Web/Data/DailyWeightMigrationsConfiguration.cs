using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace TrackIt.Web.Data
{
    public class DailyWeightMigrationsConfiguration : DbMigrationsConfiguration<DailyWeightContext>
    {
        public DailyWeightMigrationsConfiguration()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DailyWeightContext context)
        {
            base.Seed(context);
#if DEBUG
            if (!context.DailyWeights.Any())
            {
                decimal weight = 230;
                var random = new Random();
                decimal bodyFat = (decimal)23.7;
                for (int i = 182; i > 0; i--)
                {
                    var dailyWeight= new DailyWeight
                        {
                            UserName = "ed",
                            Weight = weight + (decimal) (random.NextDouble() * 2),
                            WeightDate = DateTime.Today.AddDays(-1*i),
                            BodyFat = bodyFat

                        };
                    context.DailyWeights.Add(dailyWeight);
                    weight = weight - 0.2857M;
                    bodyFat = bodyFat - 0.02M;
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
            }

#endif
        }
    }
}