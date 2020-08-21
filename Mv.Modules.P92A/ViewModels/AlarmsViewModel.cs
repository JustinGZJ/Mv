using LiveCharts;
using Mv.Modules.P92A.Service;
using Mv.Ui.Mvvm;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Unity;

namespace Mv.Modules.P92A.ViewModels
{
    public class AlarmItemVm : BindableBase
    {
        private string address;

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string startTime;

        public string StartTime
        {
            get { return startTime; }
            set { SetProperty(ref startTime, value); }
        }

        string englishMessage;
        public string EnglishMessage
        {
            get { return englishMessage; }
            set { SetProperty(ref englishMessage, value); }
        }

        private int timeStamp;

        public int TimeStamp
        {
            get { return timeStamp; }
            set { SetProperty(ref timeStamp, value); }
        }
    }

    public class AlarmsViewModel : ViewModelBase
    {
        private string 报警代码;
        public string AlarmCode
        {
            get { return 报警代码; }
            set { SetProperty(ref 报警代码, value); }
        }
        private ChartValues<double> DT时间1=new  ChartValues<double>(new double[] { 1});
        public ChartValues<double> DownTime1
        {
            get { return DT时间1; }
            set { SetProperty(ref DT时间1, value); }
        }

        private ChartValues<double> DT时间2=new ChartValues<double>(new double[] { 1});
        public ChartValues<double> DownTime2
        {
            get { return DT时间2; }
            set { SetProperty(ref DT时间2, value); }
        }

        private ChartValues<double> 待机时间1=new ChartValues<double>(new double[] { 1 });
        public ChartValues<double> IdelTime1
        {
            get { return 待机时间1; }
            set { SetProperty(ref 待机时间1, value); }
        }
        private ChartValues<double> 待机时间2=new ChartValues<double>(new double[] { 1 });
        public ChartValues<double> IdelTime2
        {
            get { return 待机时间2; }
            set { SetProperty(ref 待机时间2, value); }
        }

        private ChartValues<double> 运行时间1=new ChartValues<double>(new double[] { 1});
        public ChartValues<double> Runtime1
        {
            get { return 运行时间1; }
            set { SetProperty(ref 运行时间1, value); }
        }
        private ChartValues<double> 运行时间2=new ChartValues<double>(new double[] { 1 });
        public ChartValues<double> Runtime2
        {
            get { return 运行时间2; }
            set { SetProperty(ref 运行时间2, value); }
        }
       


        private readonly IAlarmService alarmService;
        public AlarmsViewModel(IUnityContainer container, IAlarmService alarmService,IDeviceReadWriter device) : base(container)
        {
            this.alarmService = alarmService;
            Observable.Interval(TimeSpan.FromSeconds(0.5)).Subscribe(x =>
            {
                var alarmItems = alarmService.GetAlarmItems();
                Invoke(() =>
                {
                    AlarmCode= device.GetUInt(625).ToString();//925
                    DownTime1[0]=device.GetUInt(627)/1000d;//927
                    DownTime2[0]=device.GetUInt(629)/1000d;//929
                    IdelTime1[0]=device.GetUInt(631)/1000d;//931
                    IdelTime2[0]=device.GetUInt(633)/1000d;//933
                    Runtime1[0] = device.GetUInt(635)/1000d;//935
                    Runtime2[0] = device.GetUInt(637)/1000d;//937
                    AlarmItems.Clear();
                    alarmItems.ForEach(x => AlarmItems.Add(new AlarmItemVm
                    {
                        Address = x.Address,
                        Message = x.Message,
                        StartTime = x.StartTime.ToString(),
                        EnglishMessage=x.EnglishMessage,
                        TimeStamp = (int)x.TimeSpan.TotalSeconds
                    }));
                });
            });
        }

        public ObservableCollection<AlarmItemVm> AlarmItems { get; set; } = new ObservableCollection<AlarmItemVm>();
    }
}