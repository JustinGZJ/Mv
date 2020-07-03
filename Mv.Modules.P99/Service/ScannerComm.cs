using Prism.Events;
using Prism.Logging;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mv.Modules.P99.Service
{
    public class CognexScanner
    {
        private readonly string ip;
        private readonly int port;
        public int TimeOut { get; set; } = 1000;
        SimpleTcpClient tcpClient = new SimpleTcpClient();
        public CognexScanner(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            var unused = CheckConnectedAsync();
        }
        public bool IsConnected => !(tcpClient.TcpClient == null || !tcpClient.TcpClient.Connected);
        public async Task<bool> CheckConnectedAsync()
        {

            if (tcpClient.TcpClient != null && tcpClient.TcpClient.Connected)
            {
                return true;
            }
            return await Task.Run(() =>
            {
                try
                {
                    tcpClient.Connect(ip, port);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            });
        }

        public async Task<(bool, string)> GetCodeAsync()
        {
            var result = await RequestAsync("+");
            return result;
        }

        private async Task<(bool, string)> RequestAsync(string cmd)
        {
            (bool, string) result = (false, "");
            try
            {
                tcpClient.TimeOut = TimeSpan.FromMilliseconds(TimeOut);
                await CheckConnectedAsync();
                var message = await Task.Run(() =>
                {
                    return tcpClient.WriteAndGetReply(cmd);
                });

                if (message != null)
                {
                    result.Item1 = true;
                    result.Item2 = message.MessageString;
                }

            }
            catch (Exception EX)
            {
                result.Item2 = EX.Message;
            }
            return result;
        }
    }
    public struct Edge
    {
        public bool ValueChanged { get; private set; }
        private short _currentValue;
        public short CurrentValue
        {
            get => _currentValue;
            set
            {
                ValueChanged = ((_currentValue) != (value));
                _currentValue = value;
            }
        }
    }
    public class MessageEvent : PubSubEvent<string>
    { }
    public class ScannerComm : IScannerComm
    {
        private readonly ILoggerFacade logger;
        private readonly IPlcScannerComm plcScannerComm;
        private readonly IEventAggregator aggregator;
        private List<CognexScanner> conections = new List<CognexScanner>();
        IObservable<(short, short, short, short)> triggers;
        Edge[] edges = new Edge[4] { new Edge(), new Edge(), new Edge(), new Edge() };
        public ScannerComm(ILoggerFacade logger, IPlcScannerComm plcScannerComm, IEventAggregator aggregator)
        {
            this.logger = logger;
            this.plcScannerComm = plcScannerComm;
            this.aggregator = aggregator;
            conections.Add(new CognexScanner("192.168.1.51", 8000));
            conections.Add(new CognexScanner("192.168.1.52", 8000));
            conections.Add(new CognexScanner("192.168.1.53", 8000));
            conections.Add(new CognexScanner("192.168.1.54", 8000));

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        edges[i].CurrentValue= plcScannerComm.GetShort(i, 0);
                        if (edges[i].ValueChanged && edges[i].CurrentValue == 1)
                        {
                            var task = Task.Run(async () =>
                            {
                                await GetCodeAsync(0).ConfigureAwait(false);
                            });
                        }
                    }
           
                    Thread.Sleep(1);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private async Task GetCodeAsync(int index)
        {
            this.aggregator.GetEvent<MessageEvent>().Publish($"{index + 1}号扫码枪扫码");
            var code = await GetCodeAsync(index, 1000).ConfigureAwait(false);
            var result = code.Item1 ? "PASS" : "FAIL";
            aggregator.GetEvent<MessageEvent>().Publish(payload: $"{result}:{code.Item2}");
            if (code.Item1)
            {
                plcScannerComm.SetString(index, 4, code.Item2);
                plcScannerComm.SetShort(index, 0, 1);
                this.aggregator.GetEvent<MessageEvent>().Publish("等待触发信号关闭");
                SpinWait.SpinUntil(() => plcScannerComm.GetShort(index, 0) != 0,500);
                plcScannerComm.SetShort(index, 0, 0);
                this.aggregator.GetEvent<MessageEvent>().Publish("扫码结束");
            }
            else
            {
                plcScannerComm.SetShort(index, 0, 999);
            }
        }

        public async Task<(bool, string)> GetCodeAsync(int id, int timeout = 1000)
        {
            conections[id].TimeOut = timeout;
            return await conections[id].GetCodeAsync();
        }
    }
}