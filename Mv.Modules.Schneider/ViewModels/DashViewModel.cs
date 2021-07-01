using DataService;
using Mv.Core.Interfaces;
using Mv.Modules.Schneider.Service;
using Mv.Modules.Schneider.Views;
using Mv.Ui.Core;
using Mv.Ui.Mvvm;
using MV.Core.Events;
using Prism.Logging;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Runtime.InteropServices;
using Unity;
using Newtonsoft.Json;
using Modbus;
using System.Net.Sockets;
using Modbus.Device;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Threading;
using HslCommunication.ModBus;

namespace Mv.Modules.Schneider.ViewModels
{

    public class DashViewModel : ViewModelBase, IViewLoadedAndUnloadedAware<Dash>
    {
        enum ScanCode : int
        {
            STANDBY = 0,
            OK = 1,
            ERORR = 2,
            PRODUCTERR = 3,
            STATIONERR = 4,
        }

        private readonly IDataServer dataServer;
        private readonly IServerOperations operations;
        private readonly IConfigureFile configureFile;
        private readonly ILoggerFacade logger;
        private bool bgettensiongroup1;
        private bool bgettensiongroup2;
        Dictionary<string, IObservable<short>> tensionobs = new Dictionary<string, IObservable<short>>();
        UserMessageEvent userMessageEvent;
        ProductDataCollection dataCollection = null;
        ProductDataCollection uploaddata = null;
        CompositeDisposable disposables3 = new CompositeDisposable();
        Queue<List<string>> buffers = new Queue<List<string>>();



        IDevice scanner;
        private void PushMsg(string msg, Category category = Category.Debug)
        {
            Dispatcher.Invoke(() =>
            {
                userMessageEvent.Publish(new UserMessage() { Content = msg, Level = category, Source = "user" });
            });
        }
        private string barcode;
        public bool[] outputs { get; set; } = new bool[] { false, false, false, false, false };
        Subject<bool[]> RemoteIO = new Subject<bool[]>();
        string rinputs = "";
        public string Inputs { get { return rinputs; } set { SetProperty(ref rinputs, value); } }
        string routputs = "";
        public string Ontputs { get { return routputs; } set { SetProperty(ref routputs, value); } }

        public ObservableCollection<BindableWrapper<string>> Barcodes { get; private set; } = new ObservableCollection<BindableWrapper<string>>(Enumerable.Repeat(new BindableWrapper<string>() { Value = "" }, 4));
        public Queue<List<string>> Buffers { get => buffers; set => buffers = value; }
        public DashViewModel(IUnityContainer container,
                             IDataServer dataServer,
                             IServerOperations operations,
                             IConfigureFile configureFile,
                             ILoggerFacade logger) : base(container)
        {
            userMessageEvent = EventAggregator.GetEvent<UserMessageEvent>();
            scanner = new TcpDevice("192.168.1.101", 9004);
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        using (ModbusTcpNet modbus = new ModbusTcpNet("127.0.0.1") {ConnectTimeOut=100, ReceiveTimeOut=100})
                        {
                            if (!modbus.ConnectServer().IsSuccess) {
                                PushMsg("连接远程IO失败");                    
                                continue;
                            }
                            modbus.Write("16", outputs);
                            var bs = modbus.ReadBool("x=1;0", 24);
                            if (bs.IsSuccess)
                            {
                                if (bs.Content != null && bs.Content.Length > 8)
                                    RemoteIO.OnNext(bs.Content);
                            }
                            else
                            {
                                PushMsg(bs.ToMessageShowString());
                            }
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception ex)
                    {
                        PushMsg(ex.Message);
                    }

                }
            },TaskCreationOptions.LongRunning);



            Observable.Interval(TimeSpan.FromSeconds(0.5),NewThreadScheduler.Default).Subscribe(x =>
            {
                if (Buffers.Count > 0)
                {
                    outputs[2] = true;
                }
                else
                {
                    outputs[2] = false;
                }
                outputs[3] = !outputs[3];
            });
            //PLC 心跳

            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>

            (dataServer["PC_READY"])?.Write((x % 2).ToString()));

            Enumerable.Range(1, 8).Select(x => $"tension{x}").ForEach(m =>
            {
                if (dataServer[m] != null)
                    tensionobs[m] = Observable.Interval(TimeSpan.FromSeconds(0.4),NewThreadScheduler.Default).Select(x => dataServer[m].Value.Int16);
            });
            RemoteIO.ObserveOnDispatcher().Subscribe(x =>
            {
                Inputs = "输入：" + string.Join(' ', x.Take(8).Select(x => x ? 1 : 0));
                Ontputs = "输出：" + string.Join(' ', x.Skip(16).Take(8).Select(x => x ? 1 : 0));
            });

            开始绕线信号(dataServer);

            上传数据(operations);
            #region 扫码
            //扫码

            清除扫码输出();

            所有扫码完成();
            扫码(dataServer, operations, configureFile);
            #endregion

            //记录第一组张力值
            #region 记录第一组张力
            记录第一组张力();
            停止记录第一组张力(dataServer);
            #endregion
            #region 记录第二组张力
            记录第二组张力();

            停止记录第二组张力(dataServer);
            this.dataServer = dataServer;
            #endregion
            this.operations = operations;
            this.configureFile = configureFile;
            this.logger = logger;
            gettensiongroup1();
            gettensiongroup2();

            void 开始绕线信号(IDataServer dataServer)
            {
                RemoteIO.Select(x => x[3]).Rising().ObserveOn(TaskPoolScheduler.Default).Subscribe(x =>
                {
                    PushMsg("开始绕线");
                    dataCollection = new ProductDataCollection();

                    dataServer["R_Speed"].ToObservable().Select(x => x.Int32).Max().Subscribe(x =>
                    {
                        dataCollection.ProductDatas.ForEach(m => m.Velocity = x);
                    });
                    dataServer["R_Angle"].ToObservable().Select(x => x.Int32).Max().Subscribe(x =>
                    {
                        dataCollection.ProductDatas.ForEach(m => m.Angle = x);
                    });
                    if (Buffers.TryDequeue(out var barcodes))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            dataCollection.ProductDatas.ToList()[i].Code = barcodes[i];
                        }
                    }
                    else
                    {
                        PushMsg("不存在已扫过的码");
                    }
                    RaisePropertyChanged(nameof(Buffers));
                });
            }

            void 上传数据(IServerOperations operations)
            {
                RemoteIO.Select(x => x[3]).Falling().ObserveOn(TaskPoolScheduler.Default).Subscribe(x =>
                {
                    PushMsg("开始上传");
                    try
                    {
                        if (uploaddata != null)
                        {
                            foreach (var data in uploaddata.ProductDatas)
                            {
                                data.TensionGroups.First().Values = data.TensionGroups.First().Values.ToList().SkipLast(3).ToList();
                                data.TensionGroups.Last().Values = data.TensionGroups.Last().Values.ToList().SkipLast(3).ToList();
                            }
                            operations.Upload(uploaddata);
                        }
                    }
                    catch (Exception ex)
                    {
                        PushMsg(ex.GetExceptionMsg());
                    }
                });
            }

            void 清除扫码输出()
            {
                RemoteIO.Select(x => (x[0] ? 1 : 0) + (x[1] ? 2 : 0) + (x[2] ? 4 : 0))
                .Where(v => v == 0).ObserveOn(TaskPoolScheduler.Default).Subscribe(x =>
                {
                    if (outputs[0] || outputs[1])
                    {
                        PushMsg("G4清除");
                        outputs[0] = false;
                        outputs[1] = false;
                    }

                });
            }

            void 所有扫码完成()
            {
                RemoteIO.Select(x => (x[0] ? 1 : 0) + (x[1] ? 2 : 0) + (x[2] ? 4 : 0))
                    .Buffer(2, 1)
                    .Where(x => x[0] != x[1]).Select(x => x[1])
                    .Where(v => v == 5).ObserveOn(TaskPoolScheduler.Default)
              .Subscribe(x =>
              {
                  PushMsg("扫码枪完成");
                  Buffers.Enqueue(Barcodes.Select(x => x.Value).ToList());
                  RaisePropertyChanged(nameof(Buffers));
                  Barcodes.ForEach(x => x.Value = "");
                  outputs[0] = true;
                  outputs[1] = true;
              });
            }

            void 扫码(IDataServer dataServer, IServerOperations operations, IConfigureFile configureFile)
            {
                RemoteIO.Select(x => (x[0] ? 1 : 0) + (x[1] ? 2 : 0) + (x[2] ? 4 : 0)).Buffer(2, 1).Where(x => x[0] != x[1]).Select(x => x[1]).ObserveOnDispatcher()
              .Where(value => value >= 1 && value <= 4)
                 .Subscribe(m =>
                 {
                     try
                     {
                         PushMsg("扫码枪扫码");
                         if (scanner.Read("LON\r\n", out var result, TimeSpan.FromSeconds(2)) == 0)
                         {
                             PushMsg(result);
                             if (result.ToUpper().Contains("ERROR"))
                             {
                                 outputs[0] = false;
                                 dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.OK); //没扫到码
                                 PushMsg("没扫到码", Category.Warn);
                                 dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.ERORR);

                             }
                             else
                             {
                                 barcode = result.Trim('\r');
                                 Barcodes[m - 1] = barcode;
                                 // barcod
                                 if (!barcode.Contains("GDE888888") && !barcode.Contains("GDE999999") && !barcode.Contains(dataServer["PROGRAM"].ToString().Trim('\0')))
                                 {
                                     outputs[0] = false;
                                     // master.WriteSingleCoil(1, 16, false);
                                     PushMsg("机种信息不符");
                                     dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.PRODUCTERR);

                                     return;
                                 }
                                 var config = configureFile.GetValue<ScheiderConfig>(nameof(ScheiderConfig));
                                 if (operations.CheckCode($"{result},{config.MaterialCodes[m - 1]},{config.MaterialCodes[m - 1 + 4]}") == 0)
                                 {
                                     outputs[0] = true;

                                     dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.OK);
                                     PushMsg("OK!");

                                 }
                                 else
                                 {
                                     outputs[0] = false;
                                     dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.STATIONERR);
                                     PushMsg("站位验证失败", Category.Warn);

                                 }
                             }
                         }
                         else
                         {
                             outputs[0] = false;
                             dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.ERORR); //扫码枪没返回
                             PushMsg("扫码枪未连接", Category.Warn);

                         }
                     }
                     catch (Exception ex)
                     {
                         PushMsg(ex.GetExceptionMsg());
                     }
                     outputs[1] = true;
                 });
            }

            void 记录第一组张力()
            {
                RemoteIO.Select(x => x[4]).Rising().ObserveOn(TaskPoolScheduler.Default)
                    .Subscribe(x =>
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            dataCollection.TensionResut[i] = -1;
                        }
                        PushMsg("记录第一组张力");
                        if (dataCollection
                            == null)
                        {
                            dataCollection = new ProductDataCollection();
                            if (Buffers.TryDequeue(out var barcodes))
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    dataCollection.ProductDatas.ToList()[i].Code = barcodes[i];
                                }
                            }
                        }
                        bgettensiongroup1 = true;
                        // gettensiongroup1();
                    });
            }

            void 停止记录第一组张力(IDataServer dataServer)
            {
                RemoteIO.Select(x => x[4]).Falling().ObserveOn(TaskPoolScheduler.Default)
                 .Subscribe(x =>
                 {
                     PushMsg("停止记录第一组张力");
                     bgettensiongroup1 = false;
                     var standard1 = dataServer["TS1_STANDARD"].Value.Int32 * 1d;
                     var offset1 = standard1 * dataServer["TS1_OFFSET"].Value.Int32 / 1000.0d / 100d;

                     if (standard1 != 0)
                     {
                         var groups = dataCollection.ProductDatas.Select(x => x.TensionGroups.First());

                         foreach (var group in groups)
                         {
                             group.UpperLimit = (standard1 + offset1);
                             group.LowerLimit = (standard1 - offset1);
                         }

                         var gpNames = string.Join(",", groups.Select(x => x.Name));
                         PushMsg($"{gpNames}输出结果");
                         var ps = Enumerable.Range(0, 4).Zip(groups).Select(m => new
                         {
                             Index = m.First,
                             Name = m.Second.Name,
                             Values = m.Second.Values.SkipLast(3).ToList(),
                         });
                         ps.ForEach(x =>
                         {

                             PushMsg($"{x.Name}张力数据,规格{standard1}±{offset1}");
                             var values = x.Values;


                             if (values.Any())
                             {
                                 PushMsg($"最大值{values.Max(x => x.Value)}，最小值{values.Min(x => x.Value)}");
                                 if (!values.All(z => z.Value < (standard1 + offset1) && z.Value > (standard1 - offset1)))
                                 {
                                     PushMsg($"{x.Name}张力数据NG");
                                     dataCollection.TensionOutput |= (uint)(1 << x.Index);
                                 }
                                 else
                                 {
                                     PushMsg($"{x.Name}张力数据OK");
                                 }
                             }
                             else
                             {
                                 PushMsg($"{x.Name}张力数据无变化");
                             }
                         });
                         var a = dataCollection.TensionOutput & 0x0f;
                         var b = (dataCollection.TensionOutput >> 4) & 0x0f;
                         for (int i = 0; i < 4; i++)
                         {
                             dataCollection.TensionResut[i] = (dataCollection.TensionOutput & (1 << i)) > 0 ? 1 : -1;
                         }
                         dataServer["TS_OUTPUT"].Write(a | b);
                         PushMsg("tension：" + Convert.ToString(dataCollection.TensionOutput, 2).PadLeft(8, '0'));
                         PushMsg("输出：" + Convert.ToString(a | b, 2).PadLeft(4, '0'));
                         uploaddata = dataCollection;

                     }

                 });
            }

            void 记录第二组张力()
            {
                RemoteIO.Select(x => x[5]).Rising().ObserveOn(TaskPoolScheduler.Default)
               .Subscribe(x =>
               {
                   for (int i = 0; i < 4; i++)
                   {
                       dataCollection.TensionResut[i + 4] = -1;
                   }
                   PushMsg("记录第二组张力");
                   bgettensiongroup2 = true;
               });
            }

            void 停止记录第二组张力(IDataServer dataServer)
            {
                RemoteIO.Select(x => x[5]).Falling().ObserveOn(TaskPoolScheduler.Default)
           .Subscribe(x =>
           {
               PushMsg("停止记录第二组张力");
               bgettensiongroup2 = false;
               var standard2 = dataServer["TS2_STANDARD"].Value.Int32 * 1d;
               var offset2 = standard2 * (dataServer["TS2_OFFSET"].Value.Int32 / 1000.0d / 100d);

               var groups = dataCollection.ProductDatas.Select(x => x.TensionGroups.Last()).ToList();
               foreach (var group in groups)
               {
                   group.UpperLimit = (standard2 + offset2);
                   group.LowerLimit = (standard2 - offset2);
               }
               uploaddata = dataCollection;
               var gpNames = string.Join(",", groups.Select(x => x.Name));
               PushMsg($"{gpNames}输出结果");
               if (standard2 != 0)
               {
                   var ps = Enumerable.Range(4, 4).Zip(groups).Select(m => new
                   {
                       Index = m.First,
                       Name = m.Second.Name,
                       Values = m.Second.Values.SkipLast(3).ToList(),
                   });
                   ps.ForEach(x =>
                   {
                       PushMsg($"{x.Name}张力数据,规格{standard2}±{offset2}");
                       var values = x.Values;
                       if (values.Any())
                       {
                           PushMsg($"最大值{values.Max(x => x.Value)}，最小值{values.Min(x => x.Value)}");
                           if (!values.All(z => z.Value < (standard2 + offset2) && z.Value > (standard2 - offset2)))
                           {
                               PushMsg($"{x.Name}张力数据NG");
                               dataCollection.TensionOutput |= (uint)(1 << x.Index);
                           }
                           else
                           {
                               PushMsg($"{x.Name}张力数据OK");
                           }
                       }
                       else
                       {
                           PushMsg($"{x.Name}张力数据无变化");
                       }
                   });
                   var a = dataCollection.TensionOutput & 0x0f;
                   var b = (dataCollection.TensionOutput >> 4) & 0x0f;
                   for (int i = 0; i < 4; i++)
                   {
                       dataCollection.TensionResut[i + 4] = (dataCollection.TensionOutput & (1 << (i + 4))) > 0 ? 1 : -1;
                   }
                   dataServer["TS_OUTPUT"].Write(a | b);
                   PushMsg("tension：" + Convert.ToString(dataCollection.TensionOutput, 2).PadLeft(8, '0'));
                   PushMsg("输出：" + Convert.ToString(a | b, 2).PadLeft(4, '0'));

               }
           });
            }
        }

        private void gettensiongroup2()
        {
            PushMsg("开始记录第二组张力数据");

            var tensionnames = Enumerable.Range(5, 4).Select(x => $"tension{x}").ToList();
            for (int i = 0; i < 4; i++)
            {
                var tn = tensionnames[i];
                tensionobs[tn]?
                .ObserveOn(TaskPoolScheduler.Default).Subscribe(value =>
                   {
                       var groups = dataCollection?.ProductDatas.SelectMany(x => x.TensionGroups).ToList();
                       if (bgettensiongroup2)
                       {
                           groups?.FirstOrDefault(g => g.Name == tn).Values.Add(item: new ProductData.Tension() { Time = DateTime.Now, Value = value });
                       }
                   });
            }
        }

        private void gettensiongroup1()
        {
            PushMsg("开始记录第一组张力数据");


            var tensionNames = Enumerable.Range(1, 4).Select(x => $"tension{x}").ToList();
            for (int i = 0; i < 4; i++)
            {
                var m = tensionNames[i];
                tensionobs[m]?.ObserveOn(TaskPoolScheduler.Default).Subscribe(value =>
                {
                    if (bgettensiongroup1)
                    {
                        dataCollection?
                        .ProductDatas
                        .SelectMany(x => x.TensionGroups)
                        .FirstOrDefault(x => x.Name == m)
                        .Values.Add(new ProductData.Tension() { Time = DateTime.Now, Value = value });
                    }
                });
            }
        }

        public void InitChart()
        {
            var regionManager = Container.Resolve<IRegionManager>();
            if (!regionManager.Regions["TENSIONS"].Views.Any())
            {
                for (int i = 0; i < 8; i++)
                {
                    var node = $"tension{i + 1}";
                    if (tensionobs.ContainsKey(node))
                    {
                        var m = new TimeLineChartViewModel
                        {
                            Title = $"tension {i + 1}"
                        };
                        regionManager.Regions["TENSIONS"].Add(new TimeLineChart() { DataContext = m });
                        m.SetObservable(tensionobs[node].Select(x => (double)x));
                    }
                }
            }

        }

        public void OnLoaded(Dash view)
        {
            InitChart();
        }

        public void OnUnloaded(Dash view)
        {
        }
    }

    public static class IOExt
    {

        public static IObservable<bool> Rising(this IObservable<bool> observable)
        {
            return observable.Buffer(2, 1).Where(x => (x[1] == true) && (x[0] == false)).Select(x => x[1]);
        }

        public static IObservable<bool> Falling(this IObservable<bool> observable)
        {
            return observable.Buffer(2, 1).Where(x => (x[1] == false) && (x[0] == true)).Select(x => x[1]);
        }
    }
}
