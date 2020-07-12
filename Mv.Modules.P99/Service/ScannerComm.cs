using Prism.Events;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Mv.Modules.P99.Service
{
    public class MessageEvent : PubSubEvent<string>
    { }

    public class ScannerComm : IScannerComm
    {
        private readonly ILoggerFacade logger;
        private readonly IPlcScannerComm plcScannerComm;
        private readonly IEventAggregator aggregator;
        private List<CognexScanner> conections = new List<CognexScanner>();
        private Edge[] edges = new Edge[4] { new Edge(), new Edge(), new Edge(), new Edge() };

        public ScannerComm(ILoggerFacade logger, IPlcScannerComm plcScannerComm, IEventAggregator aggregator)
        {
            this.logger = logger;
            this.plcScannerComm = plcScannerComm;
            this.aggregator = aggregator;
            try
            {
                conections.Add(new CognexScanner("192.168.1.51", 8000, logger));
                conections.Add(new CognexScanner("192.168.1.52", 8000, logger));
                conections.Add(new CognexScanner("192.168.1.53", 8000, logger));
                conections.Add(new CognexScanner("192.168.1.54", 8000, logger));
            }
            catch (Exception ex)
            {
                aggregator.GetEvent<MessageEvent>().Publish(ex.Message);
              //  throw;
            }
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 4; i++)
                {
                    plcScannerComm.SetShort(i, 0, 1);
                }
                while (true)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        edges[j].CurrentValue = plcScannerComm.GetShort(j, 0);
                        if (edges[j].ValueChanged && edges[j].CurrentValue == 1)
                        {
                            var m = j;
                            var task = Task.Run(() =>
                           {
                               GetCodeAsync(m);
                           });
                        }
                    }
                    Thread.Sleep(1);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void GetCodeAsync(int index)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            plcScannerComm.SetString(index, 4, "".PadRight(20, '\0'));
            aggregator.GetEvent<MessageEvent>().Publish($"{index + 1}号扫码枪扫码");
            aggregator.GetEvent<MessageEvent>().Publish(payload: $"{index + 1}号扫码反馈信号复位");
            plcScannerComm.SetShort(index, 0, 0);
            if (!SpinWait.SpinUntil(() => plcScannerComm.GetShort(index, 1) == 0, 1000))
            {
                aggregator.GetEvent<MessageEvent>().Publish($" {index + 1}号扫码反馈信号复位写入到PLC超时");
            }
            aggregator.GetEvent<MessageEvent>().Publish($"{index + 1}号扫码等待触发信号关闭");
            if (!SpinWait.SpinUntil(() => plcScannerComm.GetShort(index, 0) == 0, 1000))
            {
                aggregator.GetEvent<MessageEvent>().Publish($"1s内{index + 1}号扫码触发信号未关闭");
                aggregator.GetEvent<MessageEvent>().Publish($"当前触发信号状态：{ plcScannerComm.GetShort(index, 0)}");
            }
            var starttick = stopwatch.ElapsedMilliseconds;
            (bool, string) code = (false, "");
            try
            {
                code = GetCodeAsync(index, 3000);
            }
            catch (Exception ex)
            {
                code = (false, ex.Message);
                ;
            }
            aggregator.GetEvent<MessageEvent>().Publish($"{index + 1}号扫码枪请求数据时间:{stopwatch.ElapsedMilliseconds}ms");
            var r = code.Item1 && (!code.Item2.Contains("ERROR"));
            var result = r ? "PASS" : "FAIL";
            aggregator.GetEvent<MessageEvent>().Publish(payload: $"{index + 1}#Scanner {result}:{code.Item2}");
            if (r)
            {
                plcScannerComm.SetString(index, 4, code.Item2.PadRight(20, '\0'));
                plcScannerComm.SetShort(index, 0, 1);
                aggregator.GetEvent<MessageEvent>().Publish($"{index + 1}号扫码结束");
            }
            else
            {
                plcScannerComm.SetString(index, 4, "".PadRight(20, '\0'));
                aggregator.GetEvent<MessageEvent>().Publish(payload: $"{index + 1}号扫码失败,{code.Item2}");
                plcScannerComm.SetShort(index, 0, 1);
            }
            if (!SpinWait.SpinUntil(() => plcScannerComm.GetShort(index, 1) == 1, 1000))
            {
                aggregator.GetEvent<MessageEvent>().Publish($" {index + 1}号扫码反馈信号置位写入到PLC超时");
            }
            stopwatch.Stop();
            aggregator.GetEvent<MessageEvent>().Publish($"{index + 1}号扫码流程所用时间{stopwatch.ElapsedMilliseconds}ms");
        }

        public (bool, string) GetCodeAsync(int id, int timeout = 1000)
        {
            conections[id].TimeOut = timeout;
            return conections[id].GetCodeAsync();
        }
    }
}