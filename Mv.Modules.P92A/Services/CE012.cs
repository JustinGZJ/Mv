﻿using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Mv.Modules.P92A.Service
{
    public class CE012 : ICE012
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

        public (bool, string) PostData(Hashtable hashtable)
        {
            try
            {
                if (hashtable == null)
                    throw new ArgumentNullException($"{nameof(PostData)}:hashtable cannot be  null");
                string postData = ParsToString(hashtable);
                string ret = Post("http://172.19.144.1:8080/AutoEquipmentDTService.asmx/AutoEquipmentDT", postData);
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
