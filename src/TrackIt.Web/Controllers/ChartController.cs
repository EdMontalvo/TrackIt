using System;
using System.IO;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Web.Mvc;
using TrackIt.Web.Data;
using TrackIt.Web.Helpers;

namespace TrackIt.Web.Controllers
{
    public class ChartController : Controller
    {
        private readonly IDailyWeightRepository _repository;

        public ChartController(IDailyWeightRepository repository)
        {
            _repository = repository;
        }


        [Authorize]
        public FileStreamResult Weight(int timeFrame = 0)
        {
            var weightChart = new Chart
            {
                Width = 600,
                Height = 300
            };
            var weights = _repository.GetWeightsByUser(User.Identity.Name);
            if (timeFrame > 0)
            {
                timeFrame = timeFrame * -1;
                var pastDate = DateTime.Today.AddMonths(timeFrame);
                weights = weights.Where(w => w.WeightDate > pastDate);
            }
            weights = weights.OrderBy(w => w.WeightDate);
            var builder = new DailyWeightChartBuilder(weights.ToList(), weightChart);
            builder.BuildChart();
            // Save the chart to a MemoryStream
            var imgStream = new MemoryStream();
            weightChart.SaveImage(imgStream, ChartImageFormat.Png);
            imgStream.Seek(0, SeekOrigin.Begin);

            // Return the contents of the Stream to the client
            return File(imgStream, "image/png");
        }
    }
}
