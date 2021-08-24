using HslCommunication.ModBus;
using MV.Core.Events;
using Prism.Events;
using Prism.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Unity;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;

using System.Reactive.Concurrency;
namespace Mv.Modules.Schneider.Service
{
    public class RemoteIOService : IRemoteIOService, IDisposable
    {
        CancellationTokenSource tokenSource;
        public bool[] outputs { get; } = new bool[] { false, false, false, false, false, false, false, false };
        bool[] _outputs { get; } = new bool[] { false, false, false, false, false, false, false, false };
        UserMessageEvent userMessageEvent;
        public event Action<bool[]> OnRecieve;
        private int lastTick;
        bool tmp;
        Task modbustask;
        ModbusTcpNet modbus;

        public RemoteIOService(IUnityContainer container)
        {
            tokenSource = new CancellationTokenSource();
            var EventAggregator = container.Resolve<IEventAggregator>();
            userMessageEvent = EventAggregator.GetEvent<UserMessageEvent>();
            lastTick = Environment.TickCount;
            int interval = 10;
            modbus = new ModbusTcpNet("192.168.1.204") { ConnectTimeOut = 1000, ReceiveTimeOut = 100 };
            if (!modbus.ConnectServer().IsSuccess)
            {
                PushMsg("连接远程IO失败");
            }
            Observable.Interval(TimeSpan.FromMilliseconds(50), NewThreadScheduler.Default).Subscribe(x =>
            {
                try
                {
                    if (outputs.Zip(_outputs).Any(X => X.First != X.Second))
                    {
                        Array.Copy(outputs, _outputs, _outputs.Length);
                        modbus.Write("16", outputs);
                        Thread.Sleep(10);
                    }

                    var bs = modbus.ReadBool("x=1;0", 24);
                    if (bs.IsSuccess)
                    {
                        if (bs.Content != null && bs.Content.Length > 8)
                            OnRecieve?.Invoke(bs.Content);
                        else
                        {
                            PushMsg("返回数据错误");
                        }
                    }
                    else
                    {
                        PushMsg(bs.ToMessageShowString());
                    }

                    if (outputs[3] != tmp)
                    {
                        tmp = outputs[3];
                        lastTick = Environment.TickCount;
                    }
                    if (Environment.TickCount - lastTick > 1000)
                    {
                        lastTick = Environment.TickCount;
                        interval = 100;
                    }
                    else
                    {
                        interval = 100;
                    }
                }
                catch (Exception ex)
                {
                    PushMsg(ex.Message);
                }
                finally
                {
                    //     modbus.ConnectClose();
                }
            });
        }

        private void PushMsg(string msg, Category category = Category.Debug)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
           {
               userMessageEvent.Publish(new UserMessage() { Content = msg, Level = category, Source = "user" });
           });
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            ((IDisposable)tokenSource).Dispose();
        }
    }
}