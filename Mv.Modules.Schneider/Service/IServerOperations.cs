using DataService;
using Mv.Core.Interfaces;
using Mv.Modules.Schneider.ViewModels;
using MV.Core.Events;
using Newtonsoft.Json;
using Prism.Events;
using Prism.Logging;
using SimpleTCP;
using System;
using System.IO;
using System.Linq;

using System.Threading.Tasks.Dataflow;

namespace Mv.Modules.Schneider.Service
{


    public interface IServerOperations
    {
        int CheckCode(string refId);
        int Upload(ProductDataCollection json);
    }
    public class ServerOperations : IServerOperations
    {

        UserMessageEvent messageEvent;
        private readonly IDataServer dataServer;
        private readonly IConfigureFile configure;
        private readonly ILoggerFacade logger;
        ScheiderConfig scheiderConfig => configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig)) ?? new ScheiderConfig();
        Func<ScheiderConfig> GetConfig => () => configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig)) ?? new ScheiderConfig();
        public string Station => scheiderConfig.Station;

        bool serverDisable => (configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig)) ?? new ScheiderConfig()).ServerDisable;
        public string Ip => (configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig)) ?? new ScheiderConfig()).ServerIP;
        public int Port { get; set; } = 502;
        public ServerOperations(IEventAggregator aggregator, IDataServer dataServer, IConfigureFile configure, ILoggerFacade logger)
        {
            messageEvent = aggregator.GetEvent<UserMessageEvent>();
            this.dataServer = dataServer;
            this.configure = configure;
            this.logger = logger;
        }

        public void OnMessage(string msg, Category level = Category.Debug)
        {
            messageEvent.Publish(new UserMessage { Content = msg, Level = level, Source = "Server" });
        }

        private int Read(string cmd, Func<string, int> func)
        {
            try
            {
                using (SimpleTcpClient client = new SimpleTcpClient())
                {
                    client.TimeOut = TimeSpan.FromSeconds(3);
                    client.Connect(Ip, Port);
                    if (!client.TcpClient.IsOnline())
                    {
                        OnMessage("没连接上.");
                        return -1;//连接失败
                    }
                    var msg = client.WriteAndGetReply(cmd);
                    OnMessage("发送数据:" + cmd);
                    if (msg != null && func != null)
                    {
                        OnMessage("收到反馈:" + msg.MessageString);
                        return func.Invoke(msg.MessageString);
                    }
                    else
                    {
                        OnMessage("没有收到返回数据");
                        return -2;//没有收到返回数据
                    }
                }
            }
            catch (Exception ex)
            {
                OnMessage(ex.Message + "\n" + ex.StackTrace, Category.Warn);
                return -3;
                //  throw;
            }
        }

        public int CheckCode(string refId)
        {
            if (serverDisable)
            {
                OnMessage("服务器验证已关闭");
                return 0;
            }
            return Read($"{Station}SC${refId}\n", (s) =>
            {
                if (!string.IsNullOrEmpty(s) && s.ToUpper().Contains("OK"))
                    return 0;
                else
                    return -4;
            });
        }
        ActionBlock<(string, ProductData)> SaveFileAction = new ActionBlock<(string, ProductData)>(x =>
        {
            var localdata = x.Item2;
            var fileName = x.Item1;
            string contents = JsonConvert.SerializeObject(localdata);
            File.WriteAllText(Path.Combine(Folders.TENSIONS, fileName), contents);
            File.WriteAllText(Path.Combine(Folders.TENSIONSBACKUP, fileName), contents);
        });
        public int Upload(ProductDataCollection uploaddataCollection)
        {
            if (serverDisable)
            {
                OnMessage("服务器验证已关闭");
                return 0;
            }
            foreach (var localdata in uploaddataCollection.ProductDatas)
            {
                localdata.Status = dataServer["R_STATUS"].Value.Int32;
                localdata.LoopTime = dataServer["R_CIRCLE"].Value.Int32 / 100f;
                localdata.Quantity = dataServer["R_QTY"].Value.Int32;
                localdata.Turns = dataServer["R_ROWS"].Value.Int32;

                localdata.Program = dataServer["PROGRAM"].ToString();
                string fileName = $"{Station}_{localdata.Code ?? ("Empty" + DateTime.Now.ToString("yyyyMMddHHmmss"))}.json";
                SaveFileAction.Post((fileName, localdata));
            }
            var uploadData = new ServerData()
            {
                Status = dataServer["R_STATUS"].Value.Int32,
                LoopTime = dataServer["R_CIRCLE"].Value.Int32 / 100f,
                Quantity = dataServer["R_QTY"].Value.Int32,
                Turns = dataServer["R_ROWS"].Value.Int32,
                Velocity = dataServer["R_Speed"].Value.Int32,
                Program = dataServer["PROGRAM"].ToString(),
                MaterialCodes = configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig)).MaterialCodes
            };
            uploadData.Codes.AddRange(uploaddataCollection.ProductDatas.Select(x => x.Code));
            var json = JsonConvert.SerializeObject(uploadData);
            return Read($"{Station}UL${json}\n", (s) =>
            {
                if (!string.IsNullOrEmpty(s) && s.ToUpper().Contains("OK"))
                    return 0;
                else
                    return -4;
            });
        }
    }
}