using DataService;
using Mv.Modules.Hmi.Service;
using Mv.Ui.Core;
using Mv.Ui.Mvvm;
using MV.Core.Events;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace Mv.Modules.Hmi.ViewModels
{
    public enum TagNames
    {
        PC_READY,   //	PC_Ready信号
        PC_SAVE,    //	PC保存完成
        PC_PRESURE, //	PC压力读取完成信号
        PC_POS, //	PC位移读取完成信号
        PC_RES, //	PC电阻读取完成信号
        PLC_SAVE,   //	PLC保存数据
        PLC_PRESURE,    //	PLC压力读取完成信号
        PLC_POS,    //	PLC位移读取完成信号
        PLC_RES,    //	PLC电阻读取完成信号
        PC_PRESURE1,    //	PC传输压力1数据
        PC_PRESURE2,    //	PC传输压力2数据
        PC_POS1,    //	PC传输位移1数据
        PC_POS2,    //	PC传输位移2数据
        PC_RESISTANCE,  //	PC传输电阻数据
        UL_PRESURE1,    //	下料传输压力1数据
        UL_PRESURE2,    //	下料传输压力2数据
        UL_POS1,    //	下料传输位移1数据
        UL_POS2,    //	下料传输位移2数据
        UL_RES, //	下料传输电阻数据
        R_PRESURE1, //	压力1_OK/NG
        R_PRESURE2, //	压力2_OK/NG
        R_POS1, //	位移1_OK/NG
        R_POS2, //	位移2_OK/NG
        R_RES,  //	电阻_OK/NG
        R_PD,   //	产品_OK/NG
        PD_NUM, //	产品型号数据
        PD_SHIFT,   //	（预留）白夜班数据
        PC_RES_ER,	//	读取电阻通信异常
        PC_POS_ER,	//	读取位置通信异常
        PC_PR_ER,	//	读取压力通信异常
    }

    public class DataModel
    {
        public DateTime Time => DateTime.Now;
        public string Product { get; set; }
        public string Presure1 { get; set; }
        public bool Presure1Result { get; set; } = true;
        public string Presure2 { get; set; }
        public bool Presure2Result { get; set; } = true;
        public string Resistance { get; set; }
        public bool ResistanceResult { get; set; } = true;
        public string Position1 { get; set; }
        public bool Position1Result { get; set; } = true;
        public string Position2 { get; set; }
        public bool Position2Result { get; set; } = true;
        public string Result { get; set; }
    }
    public class DashViewModel : ViewModelBase
    {
        IDevice presure1;
        IDevice presure2;
        IDevice resistance;
        IDevice position;
        UserMessageEvent userMessageEvent;

        public DashViewModel(IUnityContainer container, IDataServer dataServer) : base(container)
        {
            presure1 = container.Resolve<IDevice>("presure1");
            presure2 = container.Resolve<IDevice>("presure2");
            resistance = container.Resolve<IDevice>("resistance");
            position = container.Resolve<IDevice>("position");
            userMessageEvent = EventAggregator.GetEvent<UserMessageEvent>();
            Datas1.Add(new DataModel() {  Result="PASS",Position1="11", Position1Result=true});
            Datas1.Add(new DataModel() { Result = "FAIL", Position1 = "11", Position1Result = true });

            this.dataServer = dataServer;
            //PLC 心跳
            if (dataServer[GetTagName(TagNames.PC_READY)] != null)
                Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x => (dataServer[GetTagName(TagNames.PC_READY)] as BoolTag).Write(x % 2 > 0));

            //PC读取电阻值
            if (dataServer[GetTagName(TagNames.PLC_RES)] != null)
                Observable.FromEventPattern<ValueChangedEventArgs>(dataServer[GetTagName(TagNames.PLC_RES)] as BoolTag, "ValueChanged").Subscribe(x =>
                {
                    if (x.EventArgs.Value.Boolean == true)
                    {
                        userMessageEvent.Publish(new UserMessage() { Content = "开始读取电阻值", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                        try
                        {
                            if (resistance.Read("Fetch?\r\n", out string result, TimeSpan.FromMilliseconds(500)) == 0)
                            {
                                userMessageEvent.Publish(new UserMessage() { Content = result, Level = Prism.Logging.Category.Info, Source = "RESISTANCE" });
                                if (double.TryParse(result, out var number))
                                {
                                    ResistanceValue = number;
                                }
                                (dataServer[GetTagName(TagNames.PC_RES_ER)] as BoolTag).Write(false);
                                dataServer[GetTagName(TagNames.PC_RESISTANCE)].Write((int)(number * 1000));
                            }
                            else
                            {
                                dataServer[GetTagName(TagNames.PC_RESISTANCE)].Write((int)(999999));
                                (dataServer[GetTagName(TagNames.PC_RES_ER)] as BoolTag).Write(true);
                            }

                        }
                        catch (Exception ex)
                        {
                            dataServer[GetTagName(TagNames.PC_RESISTANCE)].Write((int)(999999));
                            (dataServer[GetTagName(TagNames.PC_RES_ER)] as BoolTag).Write(true);
                            userMessageEvent.Publish(new UserMessage() { Content = ex.Message, Level = Prism.Logging.Category.Exception, Source = "RESISTANCE" });
                        }
                        userMessageEvent.Publish(new UserMessage() { Content = "读取电阻值完成", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                        (dataServer[GetTagName(TagNames.PC_RES)] as BoolTag).Write(true);
                    }
                    else
                    {
                        (dataServer[GetTagName(TagNames.PC_RES_ER)] as BoolTag).Write(false);
                        (dataServer[GetTagName(TagNames.PC_RES)] as BoolTag).Write(false);
                        userMessageEvent.Publish(new UserMessage() { Content = "清除电阻值完成信号", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                    }
                });


            //PC读取压力值
            if (dataServer[GetTagName(TagNames.PLC_PRESURE)] != null)
                Observable.FromEventPattern<ValueChangedEventArgs>(dataServer[GetTagName(TagNames.PLC_PRESURE)] as BoolTag, "ValueChanged").Subscribe(x =>
                {
                    if (x.EventArgs.Value.Boolean == true)
                    {
                        userMessageEvent.Publish(new UserMessage() { Content = "开始读取压力信号", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                        try
                        {
                            if (presure1.Read(new byte[] { 0x01, 0x03, 0x00, 0x50, 0x00, 0x02, 0xC4, 0x1A }, out byte[] result, TimeSpan.FromMilliseconds(500)) >= 7)
                            {
                                userMessageEvent.Publish(new UserMessage()
                                {
                                    Content = string.Join(',', result.Select(x => Convert.ToString(x, 16)))
            ,
                                    Level = Prism.Logging.Category.Info
            ,
                                    Source = "PRESURE1"
                                });
                                if (result != null)
                                {
                                    PresureValue1 = IPAddress.HostToNetworkOrder(BitConverter.ToInt32(result, 3)); ;
                                }
                                dataServer[GetTagName(TagNames.PC_PRESURE1)].Write(PresureValue1);
                                dataServer[GetTagName(TagNames.PC_PR_ER)].Write(false);
                            }
                            else
                            {
                                dataServer[GetTagName(TagNames.PC_PR_ER)].Write(true);
                            }

                            if (presure2.Read(new byte[] { 0x01, 0x03, 0x00, 0x50, 0x00, 0x02, 0xC4, 0x1A }, out byte[] result2, TimeSpan.FromMilliseconds(500)) >= 7)
                            {
                                userMessageEvent.Publish(new UserMessage()
                                {
                                    Content = String.Join(',', result.Select(x => Convert.ToString(x, 16)))
                                    ,
                                    Level = Prism.Logging.Category.Info
                                    ,
                                    Source = "PRESURE2"
                                });
                                if (result != null)
                                {
                                    PresureValue2 = IPAddress.HostToNetworkOrder(BitConverter.ToInt32(result2, 3)); ;
                                }
                                dataServer[GetTagName(TagNames.PC_PRESURE2)].Write(PresureValue2);
                            }
                            else
                            {
                                dataServer[GetTagName(TagNames.PC_PR_ER)].Write(true);
                            }

                        }
                        catch (Exception ex)
                        {
                            userMessageEvent.Publish(new UserMessage() { Content = ex.Message, Level = Prism.Logging.Category.Exception, Source = "PRESURE" });
                            dataServer[GetTagName(TagNames.PC_PR_ER)].Write(true);
                        }
                        userMessageEvent.Publish(new UserMessage() { Content = "读取压力结束", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                        dataServer[GetTagName(TagNames.PC_PRESURE)].Write(true);
                    }
                    else
                    {
                        dataServer[GetTagName(TagNames.PC_PRESURE)].Write(false);
                        dataServer[GetTagName(TagNames.PC_PR_ER)].Write(false);
                        userMessageEvent.Publish(new UserMessage() { Content = "清除压力读取读取完成信号", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                    }
                });
            if (dataServer[GetTagName(TagNames.PLC_POS)] != null)
                Observable.FromEventPattern<ValueChangedEventArgs>(dataServer[GetTagName(TagNames.PLC_POS)] as BoolTag, "ValueChanged").ObserveOnDispatcher().Subscribe(x =>
                {
                    if (x.EventArgs.Value.Boolean == true)
                    {
                        userMessageEvent.Publish(new UserMessage() { Content = "开始读取位置信号", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                        if (position.Read("M0\r\n", out var result, TimeSpan.FromMilliseconds(800)) == 0)
                        {

                            userMessageEvent.Publish(new UserMessage() { Content = result, Level = Prism.Logging.Category.Info, Source = "位置信息" });
                            if (result.StartsWith("M0"))
                            {
                                if (result.Split(',').Length > 2)
                                {
                                    
                                    var sps = result.Split(',').Skip(1).Take(2).ToList();
                                    if (sps.Count == 2)
                                    {

                                        if (double.TryParse(sps[0], out var pos1))
                                        {
                                            PositionValue1 = pos1 / 100;
                                            dataServer[GetTagName(TagNames.PC_POS1)].Write((int)(pos1));
                                        }
                                        else
                                        {
                                            PositionValue2 = 999999;
                                            dataServer[GetTagName(TagNames.PC_POS2)].Write((int)(999999));
                                            dataServer[GetTagName(TagNames.PC_POS1)].Write((int)(999999));
                                            dataServer[GetTagName(TagNames.PC_POS_ER)].Write(true);
                                        }

                                        if (double.TryParse(sps[1], out var pos2))
                                        {
                                            PositionValue2 = pos2 / 100;
                                            dataServer[GetTagName(TagNames.PC_POS2)].Write((int)(pos2));
                                        }
                                        else
                                        {
                                            PositionValue2 = 999999;
                                            dataServer[GetTagName(TagNames.PC_POS2)].Write((int)(999999));
                                            dataServer[GetTagName(TagNames.PC_POS1)].Write((int)(999999));
                                            dataServer[GetTagName(TagNames.PC_POS_ER)].Write(true);
                                        }
                                        dataServer[GetTagName(TagNames.PC_POS2)].Write((int)(PositionValue2 * 100));
                                    }
                                }
                            }
                        }
                        else
                        {
                            userMessageEvent.Publish(new UserMessage() { Content = "没收到数据", Level = Prism.Logging.Category.Debug, Source = "POSITION" });
                        }
                        userMessageEvent.Publish(new UserMessage() { Content = "读取位置结束", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                        dataServer[GetTagName(TagNames.PC_POS)].Write(true);
                    }
                    else
                    {
                        dataServer[GetTagName(TagNames.PC_POS)].Write(false);
                        dataServer[GetTagName(TagNames.PC_POS_ER)].Write(false);
                        userMessageEvent.Publish(new UserMessage() { Content = "清除位置读取完成信号", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                    }
                });
            if (dataServer[GetTagName(TagNames.PLC_SAVE)] != null)
                Observable.FromEventPattern<ValueChangedEventArgs>(dataServer[GetTagName(TagNames.PLC_SAVE)] as BoolTag, "ValueChanged").Subscribe(x =>
                {
                    if (x.EventArgs.Value.Boolean == true)
                    {
                        userMessageEvent.Publish(new UserMessage() { Content = "开始保存数据", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                        var data = new DataModel();
                        data.Position1 = dataServer[GetTagName(TagNames.UL_POS1)].ToString();
                        data.Position2 = dataServer[GetTagName(TagNames.UL_POS2)].ToString();
                        data.Presure1 = dataServer[GetTagName(TagNames.UL_PRESURE1)].ToString();
                        data.Presure2 = dataServer[GetTagName(TagNames.UL_PRESURE2)].ToString();
                        data.Resistance = dataServer[GetTagName(TagNames.UL_RES)].ToString();
                        data.Presure1Result = dataServer[GetTagName(TagNames.R_PRESURE1)].Value.Int16 == 0;
                        data.Presure2Result = dataServer[GetTagName(TagNames.R_PRESURE2)].Value.Int16 == 0;
                        data.Position1Result = dataServer[GetTagName(TagNames.R_POS1)].Value.Int16 == 0;
                        data.Position2Result = dataServer[GetTagName(TagNames.R_POS2)].Value.Int16 == 0;
                        data.ResistanceResult = dataServer[GetTagName(TagNames.R_RES)].Value.Int16 == 0;
                        data.Product = dataServer[GetTagName(TagNames.PD_NUM)].Value.Int16 == 1 ? "B1" : "W2";
                        data.Result = dataServer[GetTagName(TagNames.R_PD)].Value.Int16 == 0 ? "PASS" : "FAIL";
                        Invoke(() =>
                        {
                            Datas1.Add(data);
                            if (Datas1.Count > 500)
                                Datas1.RemoveAt(0);
                        });

                        var dic = new Dictionary<string, string>()
                        {
                            ["时间"] = data.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                            ["产品"] = data.Product,
                            ["判定"] = data.Result,
                            ["压力1(N)"] = data.Presure1,
                            ["压力1判定"] = data.Presure1Result ? "PASS" : "FAIL",
                            ["压力2(N)"] = data.Presure2,
                            ["压力2判定"] = data.Presure2Result ? "PASS" : "FAIL",
                            ["位置1(mm)"] = data.Position1,
                            ["位置1判定"] = data.Position1Result ? "PASS" : "FAIL",
                            ["位置2(mm)"] = data.Position2,
                            ["位置2判定"] = data.Position2Result ? "PASS" : "FAIL",
                            ["电阻(ohm)"] = data.Resistance,
                            ["电阻判定"] = data.ResistanceResult ? "PASS" : "FAIL"
                        };
                        Helper.SaveFile($"./生产信息/{DateTime.Today:yyyyMMdd}.csv", dic);
                        userMessageEvent.Publish(new UserMessage() { Content = "保存数据结束", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                        dataServer[GetTagName(TagNames.PC_SAVE)].Write(true);
                    }
                    else
                    {
                        dataServer[GetTagName(TagNames.PC_SAVE)].Write(false);

                        userMessageEvent.Publish(new UserMessage() { Content = "清除保存完成信号", Level = Prism.Logging.Category.Debug, Source = "PLC" });
                    }

                });
        }


        private string GetTagName(TagNames tagenum)
        {
            return Enum.GetName(typeof(TagNames), tagenum);
        }

        private int presureValue1 = -1;
        public int PresureValue1
        {
            get { return presureValue1; }
            set { SetProperty(ref presureValue1, value); }
        }
        private int presureValue2 = -1;
        public int PresureValue2
        {
            get { return presureValue2; }
            set { SetProperty(ref presureValue2, value); }
        }

        private double positionValue1 = -1;
        public double PositionValue1
        {
            get { return positionValue1; }
            set { SetProperty(ref positionValue1, value); }
        }

        private double positionValue2 = -1;
        public double PositionValue2
        {
            get { return positionValue2; }
            set { SetProperty(ref positionValue2, value); }
        }

        private double resistanceValue = -1;
        public double ResistanceValue
        {
            get { return resistanceValue; }
            set { SetProperty(ref resistanceValue, value); }
        }


        private DelegateCommand getpresure1;
        public DelegateCommand Getpresure1 =>
            getpresure1 ?? (getpresure1 = new DelegateCommand(() =>
            {
                Task.Run(getPresure1Impl);
            }));

        private DelegateCommand getPostion;
        public DelegateCommand GetPosition =>
            getPostion ?? (getPostion = new DelegateCommand(ExecuteGetPosition));

        void ExecuteGetPosition()
        {
            userMessageEvent.Publish(new UserMessage() { Content = "开始读取位置信号", Level = Prism.Logging.Category.Debug, Source = "PLC" });
            if (position.Read("M0\r\n", out var result, TimeSpan.FromMilliseconds(800)) == 0)
            {

                userMessageEvent.Publish(new UserMessage() { Content = result, Level = Prism.Logging.Category.Info, Source = "位置信息" });
                if (result.StartsWith("M0"))
                {
                    if (result.Split(',').Length > 2)
                    {

                        var sps = result.Split(',').Skip(1).Take(2).ToList();
                        if (sps.Count == 2)
                        {

                            if (double.TryParse(sps[0], out var pos1))
                            {
                                PositionValue1 = pos1 / 100;
                                dataServer[GetTagName(TagNames.PC_POS1)].Write((int)(pos1));
                            }
                            else
                            {
                                PositionValue2 = 999999;
                                dataServer[GetTagName(TagNames.PC_POS2)].Write((int)(999999));
                                dataServer[GetTagName(TagNames.PC_POS1)].Write((int)(999999));
                                dataServer[GetTagName(TagNames.PC_POS_ER)].Write(true);
                            }

                            if (double.TryParse(sps[1], out var pos2))
                            {
                                PositionValue2 = pos2 / 100;
                                dataServer[GetTagName(TagNames.PC_POS2)].Write((int)(pos2));
                            }
                            else
                            {
                                PositionValue2 = 999999;
                                dataServer[GetTagName(TagNames.PC_POS2)].Write((int)(999999));
                                dataServer[GetTagName(TagNames.PC_POS1)].Write((int)(999999));
                                dataServer[GetTagName(TagNames.PC_POS_ER)].Write(true);
                            }
                            dataServer[GetTagName(TagNames.PC_POS2)].Write((int)(PositionValue2 * 100));
                        }
                    }
                }
            }
        }

        private void getPresure1Impl()
        {
            try
            {
                if (presure1.Read(new byte[] { 0x01, 0x03, 0x00, 0x50, 0x00, 0x02, 0xC4, 0x1A }, out byte[] result, TimeSpan.FromMilliseconds(100)) >= 7)
                {
                    userMessageEvent.Publish(new UserMessage()
                    {
                        Content = string.Join(',', result.Select(x => Convert.ToString(x, 16)))
,
                        Level = Prism.Logging.Category.Info
,
                        Source = "PRESURE1"
                    });
                    if (result != null)
                    {
                        PresureValue1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(result, 3));
                    }
                }
            }
            catch (Exception ex)
            {
                userMessageEvent.Publish(new UserMessage() { Content = ex.Message, Level = Prism.Logging.Category.Exception, Source = "PRESURE1" });
            }
        }

        private DelegateCommand getpresure2;
        public DelegateCommand Getpresure2 =>
            getpresure2 ?? (getpresure2 = new DelegateCommand(() =>
            {
                Task.Run(getPresure2Impl);
            }));

        private void getPresure2Impl()
        {
            try
            {
                if (presure2.Read(new byte[] { 0x01, 0x03, 0x00, 0x50, 0x00, 0x02, 0xC4, 0x1A }, out byte[] result, TimeSpan.FromMilliseconds(100)) >= 7)
                {
                    userMessageEvent.Publish(new UserMessage()
                    {
                        Content = String.Join(',', result.Select(x => Convert.ToString(x, 16)))
                        ,
                        Level = Prism.Logging.Category.Info
                        ,
                        Source = "PRESURE2"
                    });
                    if (result != null)
                    {
                        PresureValue2 = IPAddress.HostToNetworkOrder(BitConverter.ToInt32(result, 3));
                        // PresureValue2 =;
                    }
                }
            }
            catch (Exception ex)
            {
                userMessageEvent.Publish(new UserMessage() { Content = ex.Message, Level = Prism.Logging.Category.Exception, Source = "PRESURE2" });
            }
        }

        private DelegateCommand getResistance;
        private readonly IDataServer dataServer;

        public DelegateCommand GetResistance =>
            getResistance ?? (getResistance = new DelegateCommand(() =>
            {
                Task.Run(getResistanceImpl);
            }));

        public ObservableCollection<DataModel> Datas1 { get; set; } = new ObservableCollection<DataModel>();

        private void getResistanceImpl()
        {
            try
            {
                if (resistance.Read("Fetch?\r\n", out string result, TimeSpan.FromMilliseconds(500)) == 0)
                {
                    userMessageEvent.Publish(new UserMessage() { Content = result, Level = Prism.Logging.Category.Info, Source = "RESISTANCE" });
                    if (double.TryParse(result, out var number))
                    {
                        ResistanceValue = number;
                    }
                }
            }
            catch (Exception ex)
            {
                userMessageEvent.Publish(new UserMessage() { Content = ex.Message, Level = Prism.Logging.Category.Exception, Source = "RESISTANCE" });
            }
        }
    }
}
