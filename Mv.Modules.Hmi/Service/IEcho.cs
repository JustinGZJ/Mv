using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using SimpleTCP;

namespace Mv.Modules.Hmi.Service
{
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
            client.Connect(ip, port);
            var message = client.WriteAndGetReply(cmd);
            if (message != null)
            {
                result = message.MessageString;
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
            result= mReply?.Data;
            return mReply!=null?mReply.Data.Length:-1;
        }
    }
}
