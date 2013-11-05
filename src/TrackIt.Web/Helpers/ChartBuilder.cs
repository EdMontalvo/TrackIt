using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace TrackIt.Web.Helpers
{
    public abstract class ChartBuilder
    {
        protected Chart Chart;
        protected int NumberOfSeries;
        internal readonly Color DarkGrey = Color.FromArgb(197, 201, 212);
        internal readonly Color LightGrey = Color.FromArgb(203, 210, 216);
        internal readonly Color Black = Color.FromArgb(67, 69, 71);

        protected ChartBuilder(Chart chart, int numberOfSeries)
        {
            Chart = chart;
            NumberOfSeries = numberOfSeries;
        }

        protected virtual bool AutoScale { get; set; }

        protected int MaximumYScale { get; set; }

        protected int MinimumYScale { get; set; }

        public void BuildChart()
        {
            Chart.BackColor = DarkGrey;
            Chart.BackSecondaryColor = LightGrey;
            //Chart.Palette = ChartColorPalette.BrightPastel;
            Chart.BorderlineDashStyle = ChartDashStyle.Solid;
            Chart.BackGradientStyle = GradientStyle.TopBottom;
            Chart.BorderSkin = new BorderSkin
            {
                SkinStyle = BorderSkinStyle.Raised
            };
            Chart.ChartAreas.Add(BuildChartArea());
            var title = BuildChartTitle();
            if (!String.IsNullOrWhiteSpace(title.Text))
            {
                Chart.Titles.Add(title);
            }
            if (NumberOfSeries > 1)
            {
                Chart.Legends.Add(BuildChartLegend());
            }
            foreach (var series in BuildChartSeries())
            {
                Chart.Series.Add(series);
            }
        }

        protected virtual void CustomizeChartArea(ChartArea area)
        {

        }

        protected virtual void CustomizeChartLegend(Legend legend)
        {

        }

        protected virtual void CustomizeChartSeries(IList<Series> seriesList)
        {

        }

        protected virtual void CustomizeChartTitle(Title title)
        {

        }

        private Legend BuildChartLegend()
        {
            var legend = new Legend
            {
                Alignment = StringAlignment.Near,
                Docking = Docking.Right
            };
            CustomizeChartLegend(legend);
            return legend;
        }

        private Title BuildChartTitle()
        {
            var title = new Title
            {
                Docking = Docking.Top,
                Font = new Font("Trebuchet MS", 18.0f, FontStyle.Bold),
            };
            CustomizeChartTitle(title);
            return title;
        }

        private IEnumerable<Series> BuildChartSeries()
        {
            var seriesList = new List<Series>();
            for (int i = 0; i < NumberOfSeries; i++)
            {
                var shadowColor = Black;
                var borderColor = Color.FromArgb(64, 117, 229);
                var foreColor = Color.FromArgb(64, 117, 229);
                var series = new Series
                {
                    ChartType = SeriesChartType.Line,
                    MarkerSize = 1,
                    BorderWidth = 2,
                    MarkerStyle = MarkerStyle.None,
                    ShadowOffset = 1,
                    ShadowColor = shadowColor,
                    BorderColor = borderColor,
                    Color = foreColor,
                    IsValueShownAsLabel = false
                };
                seriesList.Add(series);
            }
            CustomizeChartSeries(seriesList);
            return seriesList;
        }

        private ChartArea BuildChartArea()
        {
            var foreColor = Black;
            var borderColor = Color.FromArgb(64, foreColor.R, foreColor.G, foreColor.B);
            var backSecondaryColor = Color.Transparent;
            var backColor = Color.Transparent;
            var shadowColor = Color.Transparent;
            var font = new Font("Trebuchet MS", 8.25F, FontStyle.Bold);

            var area = new ChartArea
            {
                BorderColor = borderColor,
                BorderDashStyle = ChartDashStyle.Solid,
                BackColor = backColor,
                BackSecondaryColor = backSecondaryColor,
                ShadowColor = shadowColor,
                BackGradientStyle = GradientStyle.None,
                Area3DStyle =
                {
                    Enable3D = false
                },
                AxisY =
                {
                    LineColor = borderColor,
                    LabelStyle =
                    {
                        Font = font,
                        ForeColor = foreColor,
                    },
                    MajorGrid =
                    {
                        LineColor = foreColor
                    },
                    IsMarginVisible = false
                },
                AxisX =
                {
                    LineColor = borderColor,
                    LabelStyle =
                    {
                        Font = font,
                        ForeColor = foreColor
                    },
                    MajorGrid =
                    {
                        LineColor = foreColor
                    },
                    IsMarginVisible = false
                }
            };
            CustomizeChartArea(area);
            return area;
        }

    }
}