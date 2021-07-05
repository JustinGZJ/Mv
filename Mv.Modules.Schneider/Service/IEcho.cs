using SimpleTCP;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Mv.Modules.Schneider.Service
{
    public static class Folders
    {
        static Folders()
        {
            Directory.CreateDirectory(TENSIONS);
            Directory.CreateDirectory(TENSIONSBACKUP);
        }
        public static readonly string TENSIONS = Path.Combine(@"D:\DATA", "TENSIONS");
        public static readonly string TENSIONSBACKUP = Path.Combine(@"D:\DATA", "TENSIONS", "BACKUP");
    }

    public static class TcpClientEx
    {
        public static bool IsOnline(this TcpClient c)
        {
            return !(c == null || (c.Client.Poll(100, SelectMode.SelectRead) && (c.Client.Available == 0)) || !c.Client.Connected);
        }
    }
    public interface IDevice
    {
        int Read(string cmd, out string result, TimeSpan timout);
        int Read(byte[] cmd, out byte[] result, TimeSpan timeout);
    }

    public class TcpDevice : IDevice
    {

        public TcpDevice(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        private readonly string ip;
        private readonly int port;

        public int Read(string cmd, out string result, TimeSpan timout)
        {
            using var client = new SimpleTcpClient() { TimeOut = timout };
            //   var tk = client.TcpClient.Client.ConnectAsync(ip, port);
            //  Task.WaitAny(tk, Task.Delay(1000));
            client.Connect(ip, port);
            if (!client.TcpClient.IsOnline())
            {
                result = "";
                return -1;
            }
            var message = client.WriteAndGetReply(cmd);
            if (message != null)
            {
                result = message.MessageString;
                if (result.Contains("OK,LON"))
                {

                    Message mReply = null;
                    client.DataReceived += (s, e) =>
                    {
                        mReply = e;
                    };
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (mReply == null && stopwatch.Elapsed < timout)
                    {
                        Thread.Sleep(10);
                    }
                    if (mReply != null)
                    {
                        result = mReply.MessageString;
                        return 0;
                    }
                    else
                    {
                        return -2;
                    }

                }
                return 0;
            }
            else
            {
                result = "";
            }
            return -1;
        }

        public int Read(byte[] cmd, out byte[] result, TimeSpan timeout)
        {
            using var client = new SimpleTcpClient() { TimeOut = timeout };
            client.Connect(ip, port);
            Message mReply = null;
            client.DataReceived += delegate (object s, Message e)
            {
                mReply = e;
            };
            client.Write(cmd);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (mReply == null && stopwatch.Elapsed < timeout)
            {
                Thread.Sleep(10);
            }
            result = mReply?.Data;
            return mReply != null ? mReply.Data.Length : -1;
        }
    }
}
