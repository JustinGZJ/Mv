using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using Prism.Mvvm;

namespace Mv.Modules.Schneider.ViewModels
{
    public class TimeLineChartViewModel:BindableBase
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        GearedValues<DateTimePoint> values = new GearedValues<DateTimePoint>();
        IDisposable disposable;
        public TimeLineChartViewModel()
        {
            Series = new SeriesCollection();
            Series.Add(new GLineSeries
            {
                Values = values.WithQuality(Quality.Low),
                Fill = Brushes.Transparent,
                LineSmoothness = 0,
                StrokeThickness = 1,
                PointGeometry = null,              
            });

        }
        public Func<double, string> YFormater => (x) => Math.Round(x, 2).ToString();
        public void SetObservable(IObservable<double> observable)
        {
            observable.ObserveOnDispatcher()
                .Subscribe(x =>
                {
                    values.Add(new DateTimePoint(DateTime.Now, x));
                    if (values.Count > 1000)
                        values.RemoveAt(0);
                }
                );
        }
        public string Name { get; } = "";
        public SeriesCollection Series { get; set; }
    }
}
