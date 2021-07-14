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
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using Unity;

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
        Dictionary<string, IObservable<ushort>> tensionobs = new Dictionary<string, IObservable<ushort>>();
        UserMessageEvent userMessageEvent;
        ProductDataCollection dataCollection = null;
        ProductDataCollection uploaddata = null;
        CompositeDisposable disposables3 = new CompositeDisposable();
        Queue<List<string>> buffers = new Queue<List<string>>();

        IObservable<bool[]> RemoteIO;
        string rinputs = "";
        public string Inputs { get { return rinputs; } set { SetProperty(ref rinputs, value); } }
        string routputs = "";
        public string Ontputs { get { return routputs; } set { SetProperty(ref routputs, value); } }

        private List<TesionMonitor> monitors = new List<TesionMonitor>();


        IDevice scanner;
        private void PushMsg(string msg, Category category = Category.Debug)
        {
            Dispatcher.Invoke(() =>
            {
                userMessageEvent.Publish(new UserMessage() { Content = msg, Level = category, Source = "user" });
            });
        }

        private string barcode;
        public ObservableCollection<BindableWrapper<string>> Barcodes { get; private set; } = new ObservableCollection<BindableWrapper<string>>(Enumerable.Repeat(new BindableWrapper<string>() { Value = "" }, 4));
        public Queue<List<string>> Buffers { get => buffers; set => buffers = value; }

        public DashViewModel(IUnityContainer container,
                             IDataServer dataServer,
                             IServerOperations operations,
                             IConfigureFile configureFile,
                             IRemoteIOService remoteIO,
                             ILoggerFacade logger) : base(container)
        {
            userMessageEvent = EventAggregator.GetEvent<UserMessageEvent>();
            scanner = new TcpDevice("192.168.1.101", 9004);

            RemoteIO = Observable.FromEvent<bool[]>(x => remoteIO.OnRecieve += x, x => remoteIO.OnRecieve -= x);
            monitors.Add(new TesionMonitor("192.168.1.200", 32));

            Observable.Interval(TimeSpan.FromSeconds(0.5), ThreadPoolScheduler.Instance).Subscribe(x =>
            {
                if (Buffers.Count > 0)
                {
                    remoteIO.outputs[2] = true;
                }
                else
                {
                    remoteIO.outputs[2] = false;
                }
                remoteIO.outputs[3] = !remoteIO.outputs[3];
            });

            //PLC 心跳

            //Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>

            //(dataServer["PC_READY"])?.Write((x % 2).ToString()));

            Enumerable.Range(1, 1).Select(x => (index: x - 1, name: $"tension{x}")).ForEach(m =>
            {
                tensionobs[m.name] = monitors[m.index].GetObservable();
            });

            RemoteIO.Subscribe(x =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Inputs = "输入：" + string.Join(' ', x.Take(8).Select(x => x ? 1 : 0));
                    Ontputs = "输出：" + string.Join(' ', x.Skip(16).Take(8).Select(x => x ? 1 : 0));
                }), null);
            });


            RemoteIO.Select(x => x[3]).Rising().ObserveOn(NewThreadScheduler.Default).Subscribe(x =>
            {
                PushMsg("开始绕线");
                dataCollection = new ProductDataCollection();
                disposables3 = new CompositeDisposable();
                disposables3.Add(
                        dataServer["R_Speed"].ToObservable().Select(x => x.Int32).Max().Subscribe(x =>
                        {
                            dataCollection.ProductDatas.ForEach(m => m.Velocity = x);
                        })
                );
                disposables3.Add(
                          dataServer["R_Angle"].ToObservable().Select(x => x.Int32).Max().Subscribe(x =>
                          {
                              dataCollection.ProductDatas.ForEach(m => m.Angle = x);
                          })
                    );

                if (Buffers.TryDequeue(out var barcodes))
                {
                    for (int i = 0; i < 1; i++)
                    {
                        dataCollection.ProductDatas.ToList()[i].Code = barcodes[i];
                    }
                }
                else
                {
                    PushMsg("不存在已扫过的码");
                }
                dataServer["CNT"].Write(Buffers.Count);
                RaisePropertyChanged(nameof(Buffers));
            });

            RemoteIO.Select(x => x[3]).Falling().ObserveOn(NewThreadScheduler.Default).Subscribe(x =>
            {
                PushMsg("上传数据");
                disposables3.Dispose();
                try
                {
                    if (dataCollection != null)
                    {
                        operations.Upload(dataCollection);

                    }
                }
                catch (Exception ex)
                {
                    PushMsg(ex.GetExceptionMsg());
                }
            });
            #region 扫码
            //扫码

            RemoteIO.Select(x => (x[0] ? 1 : 0) + (x[1] ? 2 : 0) + (x[2] ? 4 : 0))
            .Where(v => v == 0).ObserveOn(NewThreadScheduler.Default).Subscribe(x =>
            {
                try
                {
                    if (remoteIO.outputs[0] || remoteIO.outputs[1])
                    {
                        PushMsg("G4清除");
                        remoteIO.outputs[0] = false;
                        remoteIO.outputs[1] = false;
                    }
                }
                catch (Exception)
                {

                    //    throw;
                }

            });


            RemoteIO.Select(x => (x[0] ? 1 : 0) + (x[1] ? 2 : 0) + (x[2] ? 4 : 0))
                .Buffer(2, 1)
                .Where(x => x[0] != x[1]).Select(x => x[1])
                .Where(v => v == 5).ObserveOn(NewThreadScheduler.Default)
              .Subscribe(x =>
              {
                  try
                  {

                      Invoke(new Action(() =>
                      {
                          PushMsg("扫码枪完成");
                          Buffers.Clear();
                          Buffers.Enqueue(Barcodes.Select(x => x.Value).ToList());
                          RaisePropertyChanged(nameof(Buffers));
                          Barcodes.ForEach(x => x.Value = "");
                      }));

                      remoteIO.outputs[0] = true;
                      remoteIO.outputs[1] = true;
                  }
                  catch (Exception)
                  {

                      //   throw;
                  }
                  dataServer["OUT_SCAN_ERROR"]?.Write(5);

              });
            //dataServer["CLR"]?.ToObservable().Select(X => X.Int32).Where(X => X == 1).Subscribe(X =>
            //{
            //    PushMsg("清除数据");
            //    Buffers.Clear();
            //    dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.STANDBY);
            //});



            RemoteIO.Select(x => (x[0] ? 1 : 0) + (x[1] ? 2 : 0) + (x[2] ? 4 : 0)).Buffer(2, 1).Where(x => x[0] != x[1])
                .Select(x => x[1]).ObserveOn(NewThreadScheduler.Default)
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
                             remoteIO.outputs[0] = false;
                             dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.OK); //没扫到码
                             PushMsg("没扫到码", Category.Warn);
                             dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.ERORR);

                         }
                         else
                         {
                             barcode = result.Trim('\r');
                             Invoke(new Action(() =>
                             {
                                 Barcodes[m - 1] = barcode;
                             }));

                             // barcod
                             if (!barcode.Contains("GDE888888") && !barcode.Contains("GDE999999") && !barcode.Contains(dataServer["PROGRAM"].ToString().Trim('\0')))
                             {
                                 remoteIO.outputs[0] = false;
                                 // master.WriteSingleCoil(1, 16, false);
                                 PushMsg("机种信息不符");
                                 dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.PRODUCTERR);

                                 return;
                             }
                             var config = configureFile.GetValue<ScheiderConfig>(nameof(ScheiderConfig));
                             if (operations.CheckCode($"{result},{config.MaterialCodes[m - 1]},{config.MaterialCodes[m - 1 + 4]}") == 0)
                             {
                                 remoteIO.outputs[0] = true;

                                 dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.OK);
                                 PushMsg("OK!");

                             }
                             else
                             {
                                 remoteIO.outputs[0] = false;
                                 dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.STATIONERR);
                                 PushMsg("站位验证失败", Category.Warn);

                             }
                         }
                     }
                     else
                     {
                         remoteIO.outputs[0] = false;
                         dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.ERORR); //扫码枪没返回
                         PushMsg("扫码枪未连接", Category.Warn);

                     }
                 }
                 catch (Exception ex)
                 {
                     PushMsg(ex.GetExceptionMsg());
                 }
                 remoteIO.outputs[1] = true;
             });
            #endregion

            //记录第一组张力值
            #region 记录第一组张力
            RemoteIO.Select(x => x[4]).Rising().ObserveOn(NewThreadScheduler.Default)
           .Subscribe(x =>
           {
               try
               {
                   for (int i = 0; i < 1; i++)
                   {
                       dataCollection.TensionResut[i] = -1;
                   }
                   PushMsg("记录张力");
                   if (dataCollection
                       == null)
                   {
                       dataCollection = new ProductDataCollection();
                       Dispatcher.Invoke(new Action(() =>
                       {
                           if (Buffers.TryDequeue(out var barcodes))
                           {
                               for (int i = 0; i < 1; i++)
                               {
                                   dataCollection.ProductDatas.ToList()[i].Code = barcodes[i];
                               }
                           }
                       }));

                   }
                   bgettensiongroup1 = true;
               }
               catch (Exception)
               {

                   //  throw;
               }
           });
            RemoteIO.Select(x => x[4]).Falling().ObserveOn(NewThreadScheduler.Default)
             .Subscribe(x =>
             {
                 try
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
                         var ps = Enumerable.Range(0, 1).Zip(groups).Select(m => new
                         {
                             Index = m.First,
                             Name = m.Second.Name,
                             Values = m.Second.Values.Skip(10).SkipLast(10).ToList(),
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
                         for (int i = 0; i < 1; i++)
                         {
                             dataCollection.TensionResut[i] = (dataCollection.TensionOutput & (1 << i)) > 0 ? 1 : -1;
                         }
                         dataServer["TS_OUTPUT"].Write(a | b);
                         PushMsg("tension：" + Convert.ToString(dataCollection.TensionOutput, 2).PadLeft(8, '0'));
                         PushMsg("输出：" + Convert.ToString(a | b, 2).PadLeft(4, '0'));
                         uploaddata = dataCollection;

                     }

                 }
                 catch (Exception)
                 {

                     //    throw;
                 }

             });
            #endregion

            this.operations = operations;
            this.configureFile = configureFile;
            this.logger = logger;
            gettensiongroup1();


        }

        private void gettensiongroup1()
        {
            PushMsg("开始记录张力数据");


            var tensionNames = Enumerable.Range(1, 1).Select(x => $"tension{x}").ToList();
            for (int i = 0; i < 1; i++)
            {
                var m = tensionNames[i];
                tensionobs[m]?.ObserveOn(NewThreadScheduler.Default).Subscribe(value =>
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
                for (int i = 0; i < 1; i++)
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
            //   throw new NotImplementedException();
        }

        public void OnUnloaded(Dash view)
        {
            //   throw new NotImplementedException();
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
