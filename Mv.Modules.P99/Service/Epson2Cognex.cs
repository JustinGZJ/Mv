using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Mv.Modules.P99.Service
{
    public class Epson2Cognex
    {
        SimpleTcpServer server;
        SimpleTcpClient client;
        public Epson2Cognex()
        {
            server = new SimpleTcpServer();
            client = new SimpleTcpClient();
            checkClientConnected();
            server.DataReceived += Server_DataReceived;
            client.DataReceived += Client_DataReceived;
        }

        private void checkClientConnected()
        {
            if (client.TcpClient == null || !client.TcpClient.Connected)
                client.Connect("192.168.1.110", 5000);
        }


        private void Client_DataReceived(object sender, Message e)
        {
            if (server.ConnectedClientsCount > 0)
                server.Broadcast(e.Data);
        }

        private void Server_DataReceived(object sender, Message e)
        {
            checkClientConnected();
            client.Write(e.Data);
        }
    }
}
