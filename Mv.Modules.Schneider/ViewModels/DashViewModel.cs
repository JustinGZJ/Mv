using DataService;
using Mv.Modules.Schneider.Service;
using Mv.Modules.Schneider.Views;
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
        private readonly ILoggerFacade logger;
        Dictionary<string, IObservable<short>> tensionobs = new Dictionary<string, IObservable<short>>();
        UserMessageEvent userMessageEvent;
        IDevice scanner;
        private void PushMsg(string msg, Category category = Category.Debug)
        {
            userMessageEvent.Publish(new UserMessage() { Content = msg, Level = category, Source = "user" });
        }

        private string barcode;
        public ObservableCollection<string> Barcodes { get; private set; } = new ObservableCollection<string>(Enumerable.Repeat("", 4));
        public DashViewModel(IUnityContainer container, IDataServer dataServer, IServerOperations operations, ILoggerFacade logger) : base(container)
        {
            userMessageEvent = EventAggregator.GetEvent<UserMessageEvent>();
            scanner = new TcpDevice("192.168.1.101", 9004);

            //PLC 心跳

            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>

            (dataServer["PC_READY"])?.Write((x % 2).ToString()));

            Enumerable.Range(1, 8).Select(x => $"tension{x}").ForEach(m =>
            {
                if (dataServer[m] != null)
                    tensionobs[m] = dataServer[m].ToObservable().Select(x => x.Int16);
            });


            UploadDataCollection uploaddataCollection = null;
            CompositeDisposable disposables1 = new CompositeDisposable();
            CompositeDisposable disposables2 = new CompositeDisposable();
            CompositeDisposable disposables3 = new CompositeDisposable();
            Queue<List<string>> buffers = new Queue<List<string>>();

            dataServer["BUSY"]?.ToObservable().ObserveOnDispatcher().Select(x => x.Int32).Where(x => x == 1).Subscribe(x =>
            {
                PushMsg("开始绕线");
                uploaddataCollection = new UploadDataCollection();
                disposables3 = new CompositeDisposable();
                disposables3.Add(
                        dataServer["R_Speed"].ToObservable().Select(x => x.Int32).Max().Subscribe(x =>
                        {
                            uploaddataCollection.UploadDatas.ForEach(m => m.Velocity = x);
                        })
                );
                disposables3.Add(
                          dataServer["R_Angle"].ToObservable().Select(x => x.Int32).Max().Subscribe(x =>
                          {
                              uploaddataCollection.UploadDatas.ForEach(m => m.Velocity = x);
                          })
                    );
                if (buffers.TryDequeue(out var barcodes))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        uploaddataCollection.UploadDatas.ToList()[i].Code = barcodes[i];
                    }
                }
                else
                {
                    PushMsg("不存在已扫过的码");
                }
            });

            dataServer["BUSY"]?.ToObservable().ObserveOnDispatcher().Select(x => x.Int32).Where(x => x == 0).Subscribe(x =>
                {
                    disposables3.Dispose();
                    try
                    {
                        if (uploaddataCollection != null)
                        {
                            operations.Upload(uploaddataCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        PushMsg(ex.GetExceptionMsg());
                    }
                });
            #region 扫码
            //扫码

            dataServer["IN_SCAN"]?.ToObservable().Select(x => x.Int32)
            .Where(v => v == 0).Subscribe(x =>
            {
                dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.STANDBY);
            });
            dataServer["IN_SCAN"]?.ToObservable().ObserveOnDispatcher().Select(x => x.Int32)
             .Where(value => value >= 1)
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
                             dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.OK); //没扫到码
                             PushMsg("没扫到码", Category.Warn);
                             dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.ERORR);
                         }
                         else
                         {
                             barcode = result.Trim('\r');
                             Barcodes[m - 1] = barcode;
                             if (m == 4)
                             {
                                 buffers.Enqueue(Barcodes.ToList());
                             }
                             // barcod
                             if (!barcode.Contains(dataServer["PROGRAM"].ToString().Trim('\0')))
                             {
                                 PushMsg("机种信息不符");
                                 dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.PRODUCTERR);
                                 return;
                             }
                             if (operations.CheckCode(result) == 0)
                             {

                                 dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.OK);
                                 PushMsg("OK!");

                             }
                             else
                             {
                                 dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.STATIONERR);
                                 PushMsg("站位验证失败", Category.Warn);
                             }
                         }
                     }
                     else
                     {
                         dataServer["OUT_SCAN_ERROR"]?.Write((int)ScanCode.ERORR); //扫码枪没返回
                         PushMsg("扫码枪未连接", Category.Warn);
                     }
                 }
                 catch (Exception ex)
                 {
                     PushMsg(ex.GetExceptionMsg());
                 }
             });
            #endregion

            //记录第一组张力值
            #region 记录第一组张力
            dataServer["TS1_IN"]?.ToObservable().Select(X => X.Int32)
                .Where(v => v == 1).Subscribe(x =>
                {
                    PushMsg("开始记录第一组张力数据");
                    if (uploaddataCollection == null)
                    {
                        uploaddataCollection = new UploadDataCollection();
                        if (buffers.TryDequeue(out var barcodes))
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                uploaddataCollection.UploadDatas.ToList()[i].Code = barcodes[i];
                            }
                        }
                    }
                    disposables1 = new CompositeDisposable();
                    var groups = uploaddataCollection.UploadDatas.SelectMany(x => x.Tensions);

                    IEnumerable<string> tensionNames = Enumerable.Range(1, 4).Select(x => $"tension{x}");
                    tensionNames.ForEach(m => disposables1.Add(tensionobs[m]?.Subscribe(value =>
                    {
                        groups.FirstOrDefault(x => x.Name == m).Values.Add(new UploadData.Data() { Time = DateTime.Now, Value = value });
                    })));
                });
            dataServer["TS1_IN"]?.ToObservable().Select(X => X.Int32)
            .Where(v => v == 0).Subscribe(x =>
            {
                var standard1 = dataServer["TS1_STANDARD"].Value.Int32 * 1d;
                var offset1 = standard1 * dataServer["TS1_OFFSET"].Value.Int32 / 1000.0d;
                var output = 0;
                if (standard1 != 0)
                {
                    var groups = uploaddataCollection.UploadDatas.Select(x => x.Tensions.First());
                    var gpNames = string.Join(",", groups.Select(x => x.Name));
                    PushMsg($"{gpNames}输出结果");
                    var ps = Enumerable.Range(0, 4).Zip(groups).Select(m => new
                    {
                        Index = m.First,
                        Name = m.Second.Name,
                        Values = m.Second.Values,
                        //HighValues = m.Second.Values.Where(z => z.Value > standard1 + offset1).OrderByDescending(z => z.Value),
                        //LowValues = m.Second.Values.Where(z => z.Value < standard1 - offset1).OrderBy(z => z.Value),
                        Result = m.Second.Values.All(z => z.Value < (standard1 + offset1) && z.Value > (standard1 - offset1))
                    });
                    ps.ForEach(x =>
                    {
                        PushMsg($"{x.Name}张力数据,规格{standard1}±{offset1 * 1d / 1000d}");
                        if (x.Values.Any())
                        {
                            PushMsg($"最大值{x.Values.Max(x => x.Value)}，最小值{x.Values.Min(x => x.Value)}");
                            if (!x.Result)
                            {
                                PushMsg($"{x.Name}张力数据NG");
                                output += 1 << x.Index;
                            }
                            else
                            {
                                PushMsg($"{x.Name}张力数据OK");
                            }
                        }
                        else
                        {
                            PushMsg($"{x.Name}张力数据无变化");
                            output += 1 << x.Index;
                        }
                    });
                    dataServer["TS_OUTPUT"].Write(output);
                    PushMsg("输出：" + output.ToString("X2"));
                }

                disposables1.Dispose();
            });
            #endregion
            #region 记录第二组张力
            dataServer["TS2_IN"]?.ToObservable().Select(X => X.Int32)
             .Where(v => v == 1).Subscribe(x =>
             {
                 PushMsg("开始记录第二组张力数据");
                 disposables2 = new CompositeDisposable();
                 var groups = uploaddataCollection.UploadDatas.SelectMany(x => x.Tensions).ToList();
                 Enumerable.Range(5, 4).Select(x => $"tension{x}").ForEach(m => disposables2.Add(tensionobs[m]?.Subscribe(value =>
                 {
                     groups.FirstOrDefault(x => x.Name == m).Values.Add(new UploadData.Data() { Time = DateTime.Now, Value = value });
                 })));
             });

            dataServer["TS2_IN"]?.ToObservable().Select(X => X.Int32)
            .Where(v => v == 0).Subscribe(x =>
            {
                var standard2 = dataServer["TS2_STANDARD"].Value.Int32 * 1d;
                var offset2 = standard2 * (dataServer["TS2_OFFSET"].Value.Int32 / 1000.0d);
                int output = 0;
                var groups = uploaddataCollection.UploadDatas.Select(x => x.Tensions.Last()).ToList();
                var gpNames = string.Join(",", groups.Select(x => x.Name));
                PushMsg($"{gpNames}输出结果");
                if (standard2 != 0)
                {
                    var ps = Enumerable.Range(5, 4).Zip(groups).Select(m => new
                    {
                        Index = m.First,
                        Name = m.Second.Name,
                        Values = m.Second.Values,
                        Result = m.Second.Values.All(z => z.Value < (standard2 + offset2) && z.Value > (standard2 - offset2))
                    });
                    ps.ForEach(x =>
                    {
                        PushMsg($"{x.Name}张力数据,规格{standard2}±{offset2 * 1d / 1000d}");
                        if (x.Values.Any())
                        {
                            PushMsg($"最大值{x.Values.Max(x => x.Value)}，最小值{x.Values.Min(x => x.Value)}");
                            if (!x.Result)
                            {
                                PushMsg($"{x.Name}张力数据NG");
                                output += 1 << (x.Index - 4);
                            }
                            else
                            {
                                PushMsg($"{x.Name}张力数据OK");
                            }
                        }
                        else
                        {
                            PushMsg($"{x.Name}张力数据无变化");
                            output += 1 << (x.Index - 4);
                        }
                    });
                    dataServer["TS_OUTPUT"].Write(output);
                    PushMsg(output.ToString("X2"));
                }
                disposables2.Dispose();
            });
            this.dataServer = dataServer;
            #endregion

            //  

            this.operations = operations;
            this.logger = logger;
            //     InitChart();


        }

        public void InitChart()
        {
            var random = new Random();
            var regionManager = Container.Resolve<IRegionManager>();
            if (!regionManager.Regions["TENSIONS"].Views.Any())
            {
                for (int i = 0; i < 8; i++)
                {
                    var node = $"tension{i + 1}";
                    if (tensionobs.ContainsKey(node))
                    {
                        var m = new TimeLineChartViewModel();
                        m.Title = $"张力器{i + 1}";
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
}
