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

namespace Mv.Modules.Schneider.Service
{
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
        public int Port => (configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig)) ?? new ScheiderConfig()).ServerPort;
        public ServerOperations(IEventAggregator aggregator, IDataServer dataServer, IConfigureFile configure, ILoggerFacade logger)
        {
            messageEvent = aggregator.GetEvent<UserMessageEvent>();
            this.dataServer = dataServer;
            this.configure = configure;
            this.logger = logger;

        }

        public bool saveFile(string localdata, string fileName)
        {
            try
            {
                string contents = JsonConvert.SerializeObject(localdata);
                OnMessage($"{fileName}--{contents}");
                File.WriteAllText(Path.Combine(Folders.TENSIONS, fileName), contents);
                File.WriteAllText(Path.Combine(Folders.TENSIONSBACKUP, fileName), contents);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public void OnMessage(string msg, Category level = Category.Debug)
        {
            messageEvent.Publish(new UserMessage { Content = msg, Level = level, Source = "Server" });
        }

        private int Read(string cmd, Func<string, int> func)
        {
            try
            {
                using SimpleTcpClient client = new SimpleTcpClient();
                client.TimeOut = TimeSpan.FromSeconds(3);
                client.Connect(Ip, Port);
                if (!client.TcpClient.IsOnline())
                {
                   
                    OnMessage(cmd+"没连接上.");
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
                    OnMessage(cmd+"没有收到返回数据");
                    return -2;//没有收到返回数据
                }
            }
            catch (Exception ex)
            {
                OnMessage(ex.Message + "\n" + ex.StackTrace, Category.Warn);
                return -3;
                //  throw;
            }
        }


        private int HVCRead(string cmd, Func<string, int> func)
        {
            try
            {
                using SimpleTcpClient client = new SimpleTcpClient();
                client.TimeOut = TimeSpan.FromSeconds(3);
                client.Connect(Ip, Port);
                if (!client.TcpClient.IsOnline())
                {

                    OnMessage(cmd + "没连接上.");
                    return -1;//连接失败
                }
                var msg = client.WriteAndGetReply(cmd);

            //    OnMessage("发送数据:" + cmd);
                if (msg != null && func != null)
                {
             //       OnMessage("收到反馈:" + msg.MessageString);
                    return func.Invoke(msg.MessageString);
                }
                else
                {
                    OnMessage(cmd + "没有收到返回数据");
                    return -2;//没有收到返回数据
                }
            }
            catch (Exception ex)
            {
                OnMessage(ex.Message + "\n" + ex.StackTrace, Category.Warn);
                return -3;
                //  throw;
            }
        }

        private int Write(string cmd)
        {
            if (serverDisable)
            {
                OnMessage(cmd+"服务器验证已关闭");
                return 0;
            }
            try
            {
                using SimpleTcpClient client = new SimpleTcpClient();
                client.TimeOut = TimeSpan.FromSeconds(3);
                client.Connect(Ip, Port);
                if (!client.TcpClient.IsOnline())
                {
                    OnMessage("没连接上.");
                    return -1;//连接失败
                }
                client.Write(cmd);
                OnMessage("发送数据:" + cmd);
                return 0;
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

        public int Upload(ProductDataCollection uploaddataCollection)
        {
            if (serverDisable)
            {
                OnMessage("服务器验证已关闭");
            }

            var uploadData = new ServerData()
            {
                Status = dataServer["R_STATUS"].Value.Int32,
                LoopTime = dataServer["R_CIRCLE"].Value.Int32 / 100f,
                Quantity = dataServer["R_QTY"].Value.Int32,
                Turns = dataServer["R_ROWS"].Value.Int32,
                Velocity = dataServer["R_Speed"].Value.Int32,
                Program = dataServer["PROGRAM"].ToString(),
                HVC = dataServer["HVC"].Value.Int32,
                TensionOutput = uploaddataCollection.TensionOutput,
                TensionResults = uploaddataCollection.TensionResut,
                MaterialCodes = configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig)).MaterialCodes
            };
            uploadData.Codes.AddRange(uploaddataCollection.ProductDatas.Select(x => x.Code));
            var json = JsonConvert.SerializeObject(uploadData);
            Write($"{Station}UL${json}\n");
            foreach (var localdata in uploaddataCollection.ProductDatas)
            {
                try
                {
                    foreach (var group in localdata.TensionGroups)
                    {
                        group.Values = group.Values.ToList();
                    }
                    localdata.Status = dataServer["R_STATUS"].Value.Int32;
                    localdata.LoopTime = dataServer["R_CIRCLE"].Value.Int32 / 100f;
                    localdata.Quantity = dataServer["R_QTY"].Value.Int32;
                    localdata.Turns = dataServer["R_ROWS"].Value.Int32;
                    localdata.Program = dataServer["PROGRAM"].ToString();                 
                    string fileName = $"{Station}_{localdata.Code ?? ("Empty" + DateTime.Now.ToString("yyyyMMddHHmmss"))}.json";
                    string js = JsonConvert.SerializeObject(localdata);
                    var result =  saveFile(js, fileName)?-1:1;
                    string content = $"{Station}JS$,{localdata.Code},{result},{js}\n";                
                    Write(content);
  
                }
                catch (Exception ex)
                {
                    OnMessage(ex.GetExceptionMsg());
                    //  throw;
                }

            }
            return 0;
        }

        public int CheckHVC(string hvc)
        {
            if (serverDisable)
            {
                OnMessage("HVC验证已关闭");
            }
          return  HVCRead($"{Station}HVC${hvc}\n", (s) =>
            {
                if (!string.IsNullOrEmpty(s) && s.ToUpper().Contains("OK"))
                    return 0;
                else
                    return -1;
            });
        }
    }
}