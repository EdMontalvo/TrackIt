using System;

namespace TrackIt.Web.Data
{
    public class DailyWeight
    {
        public int DailyWeightId { get; set; }

        public string UserName { get; set; }

        public DateTime WeightDate { get; set; }

        public decimal Weight { get; set; }

        public decimal? BodyFat { get; set; }
    }
}