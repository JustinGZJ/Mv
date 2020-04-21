using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Mv.Modules.RD402.Service
{
    public class DebugGetSn : IGetSn
    {
        public (bool, string) getsn(Hashtable hashtable)
        {
            //   F79016205CSSTCX3S + T10N 
            var randomString = GetRandomString(17, true, false, true, false, "");
            randomString += "+" + GetRandomString(2, true, false, true, false, "");
            randomString += hashtable["axis"];
            randomString += GetRandomString(1, true, false, true, false, "");
            return (true, randomString);
        }
        public string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
    }

    public class CE012 : IGetSn
    {

        public static string Post(string url, string postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

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

        public (bool, string) getsn(Hashtable hashtable)
        {
            try
            {
                string postData = ParsToString(hashtable);
                string ret = Post("http://172.19.144.1:8011/ce012.asmx/GetSTCCoilSN", postData);
                var document = new XmlDocument();
                document.LoadXml(ret);
                XmlNode root = document.LastChild;
                var nodeList = root.ChildNodes;
                if (nodeList.Count > 1 && int.TryParse(nodeList[0].InnerText, out int result))
                {
                    if (result != 0)
                        return (false, root.InnerText);
                    else
                    {
                        return (true, nodeList[1].InnerText);
                    }
                }
                else
                {
                    return (false, "Parse Error");
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }
    }
}