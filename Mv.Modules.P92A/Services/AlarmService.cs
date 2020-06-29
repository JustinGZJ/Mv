using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Prism.Events;
using Prism.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.RegularExpressions;

namespace Mv.Modules.P92A.Service
{

    public class AlarmItem
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public string EnglishMessage { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
    }
    public class MessageEvent : PubSubEvent<string> { }
    public class AlarmService : IAlarmService
    {
        internal class AlarmInfoRecord
        {
            [Index(0)]
            public string Address { get; set; }
            [Index(1)]
            public string EnglishMessage { get; set; }
            [Index(2)]
            public string Message { get; set; }
        }
        internal class AlarmInfo
        {
            public string Address { get; set; }

            public string Message { get; set; }

            public string EnglishMessage { get; set; }

            public int AddressOffset { get; set; }

            public int BitIndex { get; set; }
        }
        private readonly ILoggerFacade logger;
        private readonly IEventAggregator eventAggregator;
        private readonly IDeviceReadWriter device;
        private readonly ICE012 ce012;
        private int addressOffset = 0;  //文件中的地址和实际地址的偏移
        private int localOffset = 0;     //软件内部的偏移
        Subject<AlarmItem> subjectAlarmItem = new Subject<AlarmItem>();
        Subject<AlarmInfo> subjectNewAlarm = new Subject<AlarmInfo>();
        private IEnumerable<AlarmInfoRecord> GetAlarmInfos(FileInfo file)
        {
            try
            {
                using var reader = new StreamReader(file.FullName, encoding: Encoding.UTF8);
                using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
                var records = csv.GetRecords<AlarmInfoRecord>()
                .Where(x => !string.IsNullOrEmpty(x.Message))
                .Where(X => regex.IsMatch(X.Address))
                .Distinct().ToList();
                return records;
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message + Environment.NewLine + ex.StackTrace, Category.Exception, Priority.None);
                return new List<AlarmInfoRecord>();
            }
        }

        Regex regex = new Regex(@"^M\d+$");
        public AlarmService(ILoggerFacade logger, IEventAggregator eventAggregator, IDeviceReadWriter device,ICE012 ce012)
        {
            this.logger = logger;
            this.eventAggregator = eventAggregator;
            this.device = device;
            this.ce012 = ce012;

         
            LoadAlarmInfos();
            Observable.Interval(TimeSpan.FromMilliseconds(100)).ObserveOnDispatcher().Subscribe(ObserveAlarms);
            subjectNewAlarm.Subscribe(m =>
            {
                logger.Log(m.Message, Category.Warn, Priority.None);
            });
            subjectAlarmItem.Buffer(TimeSpan.FromSeconds(1)).Subscribe((n) =>
            {


                n.ForEach(m =>
                {
                    logger.Log(m.Message, Category.Warn, Priority.None);
                    int v = 0;
                    if(m.Message.Contains("待机")||m.Message.Contains("运行"))
                    {
                        v = 0;
                    }
                    else
                    {
                        v = -1;
                    }
                   var hashtable = new Hashtable();
                    hashtable["status"] = v.ToString();
                   // eventAggregator.GetEvent<string>()
                    hashtable["code"] = $"Winding,L3002,SW-CE012-Tanac-Main coil Winding-005,{m.Address.Substring(1)},{m.EnglishMessage},{m.Message}";
                    var result= ce012.PostData(hashtable);
                   // var msgposter =eventAggregator.GetEvent<string>
                    logger.Log($"POST:{result.Item1},{result.Item2}", Category.Debug, Priority.None);
                });
            });
        }
        Dictionary<string, AlarmItem> currentAlarmItems = new Dictionary<string, AlarmItem>();
        bool mActive;
        private void ObserveAlarms(long m)
        {
            mActive = !mActive;
            device.SetBit(0, 0, mActive);
            //   device.GetBit(200, 1); LINQ
            var inalarms = alarms.Where(x => device.GetBit(x.AddressOffset - addressOffset + localOffset, x.BitIndex));
            var noalarms = alarms.Except(inalarms);
            //添加新的报警信息
            var newalarms = inalarms.Where(x => !currentAlarmItems.ContainsKey(x.Address)).ToList();
            newalarms.ForEach((newAlarm) => currentAlarmItems[newAlarm.Address] = new AlarmItem
            {
                Address = newAlarm.Address,
                Message = newAlarm.Message,
                EnglishMessage = newAlarm.EnglishMessage,
                StartTime = DateTime.Now,
                TimeSpan = TimeSpan.FromSeconds(0)
            });
            newalarms.ForEach(x => subjectNewAlarm.OnNext(x));


            //删除旧的报警信息
            noalarms.Where(x => currentAlarmItems.ContainsKey(x.Address)).ToList()
            .ForEach((solvedAlarm) =>
            {
                AlarmItem value = currentAlarmItems[solvedAlarm.Address];
                value.StopTime = DateTime.Now;
                value.TimeSpan = value.StopTime - value.StartTime;
                subjectAlarmItem.OnNext(value);
                currentAlarmItems.Remove(solvedAlarm.Address);
                logger.Log($"{value.Address}:{value.Message}\t开始时间：{value.StartTime:HH:mm:ss}，结束时间：{value.StopTime:HH:mm:ss},持续时间：{value.TimeSpan.TotalSeconds}S"
                    , Category.Debug
                    , Priority.None);
            });
            //更新现有报警
            currentAlarmItems.Values.ForEach(x =>
            {
                x.StopTime = DateTime.Now;
                x.TimeSpan = x.StopTime - x.StartTime;
            });

        }

        public List<AlarmItem> GetAlarmItems()
        {
            return currentAlarmItems.Values.ToList();
        }
        List<AlarmInfo> alarms = new List<AlarmInfo>();
        public void LoadAlarmInfos(string filePath = @"./Alarms/Alarms.csv")
        {
            alarms = GetAlarmInfos(new FileInfo(filePath))
                .Select(x => new AlarmInfo
                {
                    Address = x.Address,
                    Message = x.Message,
                    EnglishMessage = x.EnglishMessage,
                    AddressOffset = (int.Parse(x.Address.Substring(1)) - 1) / 16,
                    BitIndex = (int.Parse(x.Address.Substring(1)) - 1) % 16
                }).ToList();
        }
    }
}
