using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web;
using Mv.Core.Interfaces;
using Mv.Modules.RD402.Service;
using Unity;

namespace Mv.Modules.RD402.ViewModels
{
    public class ICTFactory : IFactoryInfo
    {
        private readonly IConfigureFile configure;
        private readonly IGetSn snGetter;
        private IDeviceReadWriter _device;
        private RD402Config _config;

        public ICTFactory(IConfigureFile configure,IUnityContainer container)
        {
            this.configure = configure;
            this.snGetter = container.Resolve<IGetSn>("ICT");
            this._device=container.Resolve<IDeviceReadWriter>();
            _config = configure.Load().GetValue<RD402Config>(nameof(RD402Config));
        }

        public string GetSpindle(int value)
        {
            string content1 = "ABCD";
            if (value > 16 || value < 1)
                return value.ToString();
            return content1.Substring((value - 1) / 4, 1) + ((value - 1) % 4 + 1).ToString();
        }

        public bool UploadFile(bool result, string Spindle, string MatrixCode)
        {
            var hashtable = new Dictionary<string, string>();
            hashtable["SN"] = MatrixCode;
            hashtable["Time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hashtable["Machine_number"] = _config.MachineNumber;
            hashtable["Mandrel_number"] = Spindle;
            hashtable["Station"] = "STC Winding";
            hashtable["Result"] = result ? "PASS" : "FAIL"; ;
            return RD402Helper.SaveFile(Path.Combine(_config.FileDir, MatrixCode + ".csv"), hashtable);
        }

        public string GetBarcode(string MatrixCode, RD402Config config = null, int spindle = 0)
        {
            string LineCode="", Vendor="", DayOfWeek="", WireConfig="";
            if (MatrixCode!=null&&MatrixCode.Length>21)
            {
                LineCode = "0" + MatrixCode.Substring(18, 1);
                Vendor = MatrixCode.Substring(19, 1);
                DayOfWeek = MatrixCode.Substring(6, 1);
                 WireConfig = MatrixCode.Substring(21, 1);
            }
            return $"{LineCode}{config?.MachineCode}{GetSpindle(spindle)}{DayOfWeek}{Vendor}{WireConfig}";
        }

        public (bool, string) GetSn()
        {
            var hashtable = new Hashtable();
            hashtable["lineNumber"] = _config.LineNumber;
            hashtable["moName"] = _config.Mo;
            return snGetter.getsn(hashtable);
        }

        public (bool,string) CheckStation(IEnumerable<string> Sns)
        {
            Hashtable hashtable = new Hashtable();
            hashtable["sn"] = string.Join(";", Sns)+";";
            hashtable["p"] = "CheckFerriteLink";
            hashtable["c"] = "QUERY_HISTORY";
            try
            {
                if (hashtable == null)
                    throw new ArgumentNullException($"{nameof(CheckStation)}:hashtable cannot be  null");
                string postData = ParsToString(hashtable);
                string ret = Post("http://10.33.24.21/bobcat/sfc_response.aspx", postData);
                return (true, ret);
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
            return (false, "");
        }
        public static string Post(string url, string postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Timeout = 2000;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
        private static string ParsToString(Hashtable Pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in Pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));
            }
            return sb.ToString();
        }
    }
}