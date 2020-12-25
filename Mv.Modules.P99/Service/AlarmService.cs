using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Prism.Events;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Mv.Modules.P99.Service
{


    public class AlarmItem
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
    }

    public class AlarmService : IAlarmService
    {
        internal class AlarmInfoRecord
        {
            [Index(2)]
            public string Address { get; set; }
            [Index(4)]
            public string Message { get; set; }
        }
        internal class AlarmInfo
        {
            public string Address { get; set; }

            public string Message { get; set; }

            public int AddressOffset { get; set; }

            public int BitIndex { get; set; }
        }
        private readonly ILoggerFacade logger;
        private readonly IEventAggregator eventAggregator;
        private readonly IDeviceReadWriter device;
        private int addressOffset = 3700;
        private int localOffset = 200;
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

        Regex regex = new Regex(@"^MB\d{4}[A-D0-9]{1}");
        public AlarmService(ILoggerFacade logger, IEventAggregator eventAggregator, IDeviceReadWriter device)
        {
            this.logger = logger;
            this.eventAggregator = eventAggregator;
            this.device = device;
            LoadAlarmInfos();
            
            Observable.Interval(TimeSpan.FromMilliseconds(100)).ObserveOnDispatcher().Subscribe(ObserveAlarms);
            subjectNewAlarm.Subscribe(m =>
            {
                logger.Log($"{m.Address}:{m.Message}", Category.Debug, Priority.None);
                var dictionary = new Dictionary<string, string>();
                dictionary["地址"] = m.Address;
                dictionary["开始时间"] = DateTime.Now.ToString();
                dictionary["信息"] = m.Message;
                if (m.Message.Contains("按钮"))
                    dictionary["类型"] = "U";
                else
                    dictionary["类型"] = "A";
                Helper.SaveFile($"./SFC/{DateTime.Today:yyyyMMdd}.csv", dictionary);
            });
            subjectAlarmItem.Buffer(TimeSpan.FromSeconds(1)).Subscribe((n) =>
            {
                n.ForEach(m =>
                {
                    var dictionary = new Dictionary<string, string>();
                    dictionary["开始时间"] = m.StartTime.ToString();
                    dictionary["结束时间"] = m.StopTime.ToString();
                    dictionary["报警地址"] = m.Address;
                    dictionary["报警信息"] = m.Message;
                    dictionary["持续时间"] = m.TimeSpan.ToString();
                    Helper.SaveFile($"./报警信息/{DateTime.Today:yyyyMMdd}.csv", dictionary);
                });
            });
        }
        ConcurrentDictionary<string, AlarmItem> currentAlarmItems = new ConcurrentDictionary<string, AlarmItem>();
        //bool localvalue;
        private void ObserveAlarms(long m)
        {
            //localvalue = !localvalue;
            //device.SetBit(0, 1, localvalue);
            //   device.GetBit(200, 1);
            var inalarms = alarms.Where(x => device.GetBit(x.AddressOffset - addressOffset + localOffset, x.BitIndex));
            var noalarms = alarms.Except(inalarms);
            //添加新的报警信息
            var newalarms = inalarms.Where(x => !currentAlarmItems.ContainsKey(x.Address)).ToList();
            newalarms.ForEach((newAlarm) => currentAlarmItems[newAlarm.Address] = new AlarmItem
            {
                Address = newAlarm.Address,
                Message = newAlarm.Message,
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
                currentAlarmItems.TryRemove(solvedAlarm.Address, out var removed);
              //  currentAlarmItems.Remove(solvedAlarm.Address);
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
            var list = new List<AlarmItem>();
            try
            {
                foreach (var item in currentAlarmItems)
                {
                    list.Add(item.Value);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message + ex.StackTrace,Category.Exception,Priority.None);
              // throw;
            }
            return list;
          //  return currentAlarmItems.Values.ToList();
        }
        List<AlarmInfo> alarms = new List<AlarmInfo>();
        public void LoadAlarmInfos(string filePath = @"./Alarms/Alarms.csv")
        {
            alarms = GetAlarmInfos(new FileInfo(filePath))
                .Select(x => new AlarmInfo
                {
                    Address = x.Address,
                    Message = x.Message,
                    AddressOffset = int.Parse(x.Address.Substring(2, 4)),
                    BitIndex = Convert.ToInt32(x.Address.Substring(6, 1), 16)
                }).ToList();
        }
    }
}
