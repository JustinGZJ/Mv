using DataService;
using Mv.Core;
using Mv.Core.Interfaces;
using Mv.Modules.Schneider.ViewModels;
using MV.Core.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Events;
using Prism.Logging;
using SimpleTCP;
using System;
using System.IO;
using System.Linq;

namespace Mv.Modules.Schneider.Service
{


    public interface IServerOperations
    {
        int CheckCode(string refId);
        int Upload(UploadDataCollection json);
    }
    public class ServerOperations : IServerOperations
    {

        UserMessageEvent messageEvent;
        private readonly IDataServer dataServer;
        private readonly IConfigureFile configure;

        public string Station  =>(configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig))??new ScheiderConfig()).Station;

        bool serverDisable => (configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig)) ?? new ScheiderConfig()).ServerDisable;
        public string Ip => (configure.GetValue<ScheiderConfig>(nameof(ScheiderConfig)) ?? new ScheiderConfig()).ServerIP;
        public int Port { get; set; } = 502;
        public ServerOperations(IEventAggregator aggregator, IDataServer dataServer, IConfigureFile configure)
        {
            messageEvent = aggregator.GetEvent<UserMessageEvent>();
            this.dataServer = dataServer;
            this.configure = configure;
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

        public int Upload(UploadDataCollection uploaddataCollection)
        {
            if (serverDisable)
            {
                OnMessage("服务器验证已关闭");
                return 0;
            }         
            foreach (var localdata in uploaddataCollection.UploadDatas)
            {
                localdata.Status = dataServer["R_STATUS"].Value.Int32;
                localdata.LoopTime = dataServer["R_CIRCLE"].Value.Int32 / 100f;
                localdata.Quantity = dataServer["R_QTY"].Value.Int32;
                localdata.Turns = dataServer["R_ROWS"].Value.Int32;
     
                localdata.Program = dataServer["PROGRAM"].ToString();
         
                File.WriteAllTextAsync(Path.Combine(Folders.TENSIONS, $"{Station}_{localdata.Code ?? ("Empty" + DateTime.Now.ToString("yyyyMMddHHmmss"))}.json"), JsonConvert.SerializeObject(localdata));
                File.WriteAllTextAsync(Path.Combine(Folders.TENSIONSBACKUP, $"{Station}_{localdata.Code ?? ("Empty" + DateTime.Now.ToString("yyyyMMddHHmmss"))}.json"), JsonConvert.SerializeObject(localdata));
            }
            var uploadData = new ServerData
            {
                Status = dataServer["R_STATUS"].Value.Int32,
                LoopTime = dataServer["R_CIRCLE"].Value.Int32 / 100f,
                Quantity = dataServer["R_QTY"].Value.Int32,
                Turns = dataServer["R_ROWS"].Value.Int32,
                Velocity = dataServer["R_Speed"].Value.Int32,
                Program = dataServer["PROGRAM"].ToString()
            };
            uploadData.Codes.AddRange(uploaddataCollection.UploadDatas.Select(x => x.Code));
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