using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using Mv.Core;
using Prism.Mvvm;
using Unity;

namespace Mv.Modules.Schneider.ViewModels
{
    public class TimeLineChartViewModel : BindableBase
    {
        private string title = "tension";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        GearedValues<double> values = new GearedValues<double>();
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
            Observable.Interval(TimeSpan.FromMilliseconds(300)).ObserveOnDispatcher().Subscribe(x => RaisePropertyChanged(nameof(TensionValue)));

        }
        public Func<double, string> YFormater => (x) => Math.Round(x, 0).ToString();

        private double tensionValue = 0;
        public double TensionValue
        {
            get { return tensionValue; }
            set {
                SetProperty(ref value, value); }
        }

        public void SetObservable(IObservable<double> observable)
        {
            disposable?.Dispose();
            disposable = observable.ObserveOnDispatcher()
                .Subscribe(x =>
                {

                    try
                    {
                        tensionValue = Math.Round(x, 0);
                        values.Add(tensionValue);
                        if (values.Count > 500)
                            values.RemoveAt(0);
                    }
                    catch (Exception ex)
                    {
                      
                      //  throw;
                    }
              }
                );
        }
        public string Name { get; } = "";
        public SeriesCollection Series { get; set; }
    }
}
