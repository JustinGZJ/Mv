using Prism.Logging;
using SimpleTCP;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mv.Modules.P99.Service
{
    public class CognexCommunication : ICognexCommunication
    {
        SimpleTcpClient tcpClient = new SimpleTcpClient();
        private readonly ILoggerFacade logger;

        public CognexCommunication(ILoggerFacade logger )
        {
            tcpClient.TimeOut = TimeSpan.FromSeconds(5);
            var task = CheckConnectionAsync();
            this.logger = logger;
        }

        private async Task CheckConnectionAsync()
        {
            try
            {
                if (tcpClient.TcpClient!=null&&!tcpClient.TcpClient.Connected)
                {
                    await Task.Run(() =>
                     tcpClient.Connect("127.0.0.1", 6000));
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message, Category.Exception, Priority.None);
                ;
            }
        }

        public async Task<bool> CalibrationAsync(int id, int step, (double, double, double) position)
        {
            bool result = true;
            string response;
            switch (step)
            {
                case 0:
                    response = await RequestAsync($"SC,{2 + id},9" + Environment.NewLine).ConfigureAwait(false);
                    if (response.Contains("SC,0"))
                        result = false;
                    break;

                case 1:
                    response = await RequestAsync($"C{2 + id},{position.Item1},{position.Item2},{position.Item3}" + Environment.NewLine).ConfigureAwait(false);
                    if (response.Contains($"C{2 + id},0"))
                        result = false;
                    break;

                case 2:
                    response = await RequestAsync($"EC" + Environment.NewLine).ConfigureAwait(false);
                    if (response.Contains("EC,0"))
                        result = false;
                    break;
            }
            return result;
        }

        public async Task<(bool, string, double, double)> TrainCameraAsync(int id, (double, double, double, double) pos)
        {
            var response = await RequestAsync($"L{id + 2},{pos.Item1 },{pos.Item2 },{pos.Item3 },{pos.Item4}" + Environment.NewLine).ConfigureAwait(false);

            var splits = response.Split(',');
            if (splits.Length >= 4 && splits.Skip(2).All(x => double.TryParse(x, out var value)))
            {
                var vs = splits.Skip(2).Select(m => double.Parse(m)).ToArray();
                return (true, response, vs[0], vs[1]);
            }
            else
            {
                return (false, response, 0, 0);
            }
        }

        public async Task<(bool, string, int, int, int, int,int,int,int,int)> TakePhotoAsync(int id, double x, double y)
        {
            var response = await RequestAsync($"T{2 + id},{x},{y}" + Environment.NewLine).ConfigureAwait(false);
            var splits = response.Split(',');
            if (splits.Length >= 8 && splits.Skip(2).All(x => double.TryParse(x, out var value)))
            {
                var vs = splits.Skip(2).Select(m => (int)(double.Parse(m) * 1000)).ToArray();
                return (true, response, vs[0], vs[1], vs[2], vs[3],vs[4],vs[5],vs[6],vs[7]);
            }
            else
            {
                return (false, response, 0, 0, 0, 0,0,0,0,0);
            }
        }

        private async Task<string> RequestAsync(string cmd)
        {
            var responseData = "";
            logger.Log($"发送至康耐视:{cmd}", Category.Debug, Priority.None);
            try
            {
                await CheckConnectionAsync();
                var message = await Task.Run(() =>
                  {
                      return tcpClient.WriteAndGetReply(cmd);
                  });

                if (message != null)
                    responseData = message.MessageString;
            }
            catch (Exception EX)
            {
                responseData = "CONNECT ERROR";
              //  throw;
            }
       
            return responseData;
        }
    }
}