using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using TrackIt.Web.Data;
using Chart = System.Web.UI.DataVisualization.Charting.Chart;

namespace TrackIt.Web.Helpers
{

    public class DailyWeightChartBuilder : ChartBuilder
    {
        private readonly IEnumerable<DailyWeight> _dailyWeights;

        public DailyWeightChartBuilder(IEnumerable<DailyWeight> dailyWeights, Chart chart)
            : base(chart, 1)
        {
            _dailyWeights = dailyWeights;
            AutoScale = false;
            MinimumYScale = 400;
            MaximumYScale = 400;
        }

        protected override sealed bool AutoScale { get; set; }

        protected override void CustomizeChartSeries(IList<Series> seriesList)
        {
            var min = _dailyWeights.Min(d => d.Weight);
            var max = _dailyWeights.Max(d => d.Weight);
            var maxDate = _dailyWeights.Max(d => d.WeightDate);
            var minDate = _dailyWeights.Min(d => d.WeightDate);
            int padding;

            var dateDiff = maxDate.Subtract(minDate);
            if (dateDiff.Days <= 31)
            {
                padding = 2;
            }
            else if (dateDiff.Days < 100)
            {
                padding = 5;
            }
            else
            {
                padding = 15;
            }
            MinimumYScale = Convert.ToInt32(Math.Floor(min / 5)) * 5;
            MaximumYScale = Convert.ToInt32(Math.Ceiling(max / 5)) * 5;
            Series weights = seriesList.Single();
            weights.SmartLabelStyle.Enabled = true;
            weights.Name = "Weights";
            foreach (var record in _dailyWeights)
            {
                weights.Points.AddXY(record.WeightDate, record.Weight);
            }
            var font = new Font("Trebuchet MS", 8.25F, FontStyle.Bold);
            var startAnnotation = new CalloutAnnotation
            {
                AnchorDataPoint = weights.Points[0],
                AnchorOffsetX = 5,
                AnchorOffsetY = -5,
                Text = weights.Points[0].YValues.Single().ToString("0.0"),
                ForeColor = Color.Black,
                Font = font,
                CalloutStyle = CalloutStyle.RoundedRectangle,
                Height = 7,
                Width = 7,
                BackColor = Color.FromArgb(128, Color.Green)
            };

            Chart.Annotations.Add(startAnnotation);
            var endAnnotation = new CalloutAnnotation
            {
                AnchorDataPoint = weights.Points.Last(),
                Text = weights.Points.Last().YValues.Single().ToString("0.0"),
                AnchorOffsetY = 5,
                AnchorOffsetX = 5,
                ForeColor = Color.Black,
                Font = font,
                CalloutStyle = CalloutStyle.RoundedRectangle,
                Height = 7,
                Width = 7,
                BackColor = Color.FromArgb(128, Color.Red)
            };

            Chart.Annotations.Add(endAnnotation);
            Chart.ChartAreas[0].AxisY.Minimum = (AutoScale) ? Double.NaN : MinimumYScale;
            Chart.ChartAreas[0].AxisY.Maximum = (AutoScale) ? Double.NaN : MaximumYScale;
            Chart.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-d";
            Chart.ChartAreas[0].AxisX.LabelStyle.Interval = 20;
            Chart.ChartAreas[0].AxisX.Maximum = (AutoScale) ? Double.NaN : maxDate.AddDays(padding).ToOADate();
        }

        protected override void CustomizeChartTitle(Title title)
        {
            title.Text = "";
        }
    }
}