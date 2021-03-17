using System;
using System.Reactive.Linq;
using LiveCharts;
using Prism.Mvvm;

namespace Mv.Modules.Schneider.ViewModels
{
    public class MeasureModel
    {
        public DateTime DateTime { get; set; }

        public double Value { get; set; }
    }

    public class TimeLineChartViewModel : BindableBase
    {
        private string title = "tension";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public Func<double, string> DateTimeFormatter { get; set; } = value =>
          new DateTime((long)value).ToString("T");
        public Func<double, string> YFormatter { get; set; } = value => value.ToString("f0");


        IDisposable disposable;
        public TimeLineChartViewModel()
        {


            //the values property will store our values array
            ChartValues = new ChartValues<MeasureModel>() ;
            //lets set how to display the X Labels


            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(20).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);


        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(2).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(118).Ticks; // and 8 seconds behind
        }
        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                SetProperty(ref _axisMax, value);
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                SetProperty(ref _axisMin, value);
            }
        }

        public double YAxisMax
        {
            get { return _yaxisMax; }
            set
            {
                SetProperty(ref _yaxisMax, value);
            }
        }
        public double YAxisMin
        {
            get { return _yaxisMin; }
            set
            {
                SetProperty(ref _yaxisMin, value);
            }
        }


        private double tensionValue = 0;
        private double _axisMax;
        private double _axisMin;
        private double _yaxisMax=2000;
        private double _yaxisMin=0;
        public double TensionValue
        {
            get { return tensionValue; }
            set
            {
                SetProperty(ref tensionValue, value);
            }
        }
        Random random = new Random();
        public void SetObservable(IObservable<double> observable)
        {
            disposable?.Dispose();
            disposable = observable
                .Subscribe(x =>
                {
                      TensionValue =x;
                    ChartValues.Add(new MeasureModel { DateTime = DateTime.Now, Value =TensionValue });
                    if (ChartValues.Count > 150)
                        ChartValues.RemoveAt(0);
                    SetAxisLimits(DateTime.Now);
                }
                );
        }
        public SeriesCollection Series { get; set; }
        public long AxisStep { get; private set; }
        public long AxisUnit { get; private set; }
        public ChartValues<MeasureModel> ChartValues { get; set; }
    }
}
