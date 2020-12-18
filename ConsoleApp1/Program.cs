using HslCommunication.Core;
using HslCommunication.ModBus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    public class ModbusDeviceReadWriter
    {
        byte[] rbs = new byte[400 * 2];
        byte[] wbs = new byte[40 * 2];
        public bool IsConnected { get; set; }
        ModbusTcpNet modbus;
   

        public ModbusDeviceReadWriter()
        {
            modbus = new ModbusTcpNet("127.0.0.1", 10502)
            {
                IsStringReverse = true,
                DataFormat = DataFormat.CDAB
            };

            modbus.AddressStartWithZero = true;
            Task.Factory.StartNew(() =>
            {
                modbus.ConnectServer();
                while (true)
                {
                    var rr = modbus.Read("0", (ushort)(rbs.Length / 2));
                    if (rr.IsSuccess)
                    {
                        Buffer.BlockCopy(rr.Content, 0, rbs, 0, rr.Content.Length);
                    }
                    else
                    {
            
                    }
                    IsConnected = rr.IsSuccess;
                    var wt = modbus.Write("400", wbs);
                    Thread.Sleep(100);
                }
            }, TaskCreationOptions.LongRunning);
       
        }
        public ushort GetWord(int index)
        {
            return modbus.ByteTransform.TransUInt16(rbs, index * 2);
        }
        public bool GetBit(int index, int bit)
        {
            var m = GetWord(index);
            var r = (m & (1 << bit)) > 0;
            return r;
        }

        public short GetShort(int index)
        {
            return modbus.ByteTransform.TransInt16(rbs, index * 2);
        }

        public int GetInt(int index)
        {
            return modbus.ByteTransform.TransInt32(rbs, index * 2);
        }

        public void SetInt(int index, int value)
        {
            Buffer.BlockCopy(modbus.ByteTransform.TransByte(value), 0, wbs, index * 2, 4);
        }

        public void SetShort(int index, short value)
        {
            Buffer.BlockCopy(modbus.ByteTransform.TransByte(value), 0, wbs, index * 2, 2);
        }

        public void SetBit(int index, int bit, bool value)
        {
            if (value)
            {
                var mInt16 = (ushort)(modbus.ByteTransform.TransUInt16(wbs, index * 2) | (1 << bit));
                SetShort(index, (short)mInt16);
            }
            else
            {
                var mInt16 = (ushort)(modbus.ByteTransform.TransUInt16(wbs, index * 2) & (~(1 << bit)));
                SetShort(index, (short)mInt16);
            }
        }

        public void SetString(int index, string value)
        {
            var bs = modbus.ByteTransform.TransByte(value, Encoding.ASCII);
            Buffer.BlockCopy(bs, 0, wbs, index * 2, bs.Length);
        }

        /// <summary>
        /// 从读取缓冲区读取
        /// </summary>
        /// <param name="index">字的索引</param>
        /// <param name="len">字符串长度</param>
        /// <returns></returns>
        public string GetString(int index, int len)
        {
            return modbus.ByteTransform.TransString(rbs, index, len, Encoding.ASCII);
        }

        public bool GetSetBit(int index, int bit)
        {
            return (modbus.ByteTransform.TransUInt16(wbs, index * 2) & (1 << bit)) > 0;
        }

        public void PlcConnect()
        {
            //  throw new NotImplementedException();
        }


    }

    public class PlcScannerComm 
    {

        List<(byte[], byte[])> localbs = new List<(byte[], byte[])>(4) {
             (new byte[20*2],new byte[19*2]),
            (new byte[20*2],new byte[19*2]),
            (new byte[20*2],new byte[19*2]),
            (new byte[20*2],new byte[19*2])
        };
        public bool IsConnected { get; set; }
        ModbusTcpNet modbus;
      //  private readonly ILoggerFacade logger;

        public PlcScannerComm()
        {
            modbus = new ModbusTcpNet("127.0.0.1", 10502)
            {
                DataFormat = DataFormat.CDAB,
                IsStringReverse = true
            };
            modbus.AddressStartWithZero = true;

            Task.Factory.StartNew(() =>
            {
                modbus.ConnectServer();
                while (true)
                {

                    for (int i = 0; i < 4; i++)
                    {
                        var rr = modbus.Read((0 + 20 * i).ToString(), 20);
                        if (rr.IsSuccess)
                        {
                            Buffer.BlockCopy(rr.Content, 0, localbs[i].Item1, 0, rr.Content.Length);
                        }
                        else
                        {
                        //    logger.Log(rr.Message, Category.Warn, Priority.None);
                        }
                        IsConnected = rr.IsSuccess;
                        var wt = modbus.Write((1 + 20 * i).ToString(), localbs[i].Item2);
                    }
                    Thread.Sleep(500);
                }
            }, TaskCreationOptions.LongRunning);
        }
        public ushort GetWord(int id, int index)
        {
            return modbus.ByteTransform.TransUInt16(localbs[id].Item1, index * 2);
        }
        public bool GetBit(int id, int index, int bit)
        {
            var m = GetWord(id, index);
            var r = (m & (1 << bit)) > 0;
            return r;
        }

        public short GetShort(int id, int index)
        {
            return modbus.ByteTransform.TransInt16(localbs[id].Item1, index * 2);
        }

        public uint GetUInt(int id, int index)
        {
            return modbus.ByteTransform.TransUInt32(localbs[id].Item1, index * 2);
        }

        public int GetInt(int id, int index)
        {
            return modbus.ByteTransform.TransInt32(localbs[id].Item1, index * 2);
        }

        public void SetInt(int id, int index, int value)
        {
            Buffer.BlockCopy(modbus.ByteTransform.TransByte(value), 0, localbs[id].Item2, index * 2, 4);
        }

        public void SetInt(int id, int index, uint value)
        {
            Buffer.BlockCopy(modbus.ByteTransform.TransByte(value), 0, localbs[id].Item2, index * 2, 4);
        }
        public void SetShort(int id, int index, short value)
        {
            Buffer.BlockCopy(modbus.ByteTransform.TransByte(value), 0, localbs[id].Item2, index * 2, 2);
        }

        public void SetBit(int id, int index, int bit, bool value)
        {
            if (value)
            {
                var mInt16 = (ushort)(modbus.ByteTransform.TransUInt16(localbs[id].Item2, index * 2) | (1 << bit));
                SetShort(id, index, (short)mInt16);
            }
            else
            {
                var mInt16 = (ushort)(modbus.ByteTransform.TransUInt16(localbs[id].Item2, index * 2) & (~(1 << bit)));
                SetShort(id, index, (short)mInt16);
            }
        }

        public void SetString(int id, int index, string value)
        {
            var bs = modbus.ByteTransform.TransByte(value, Encoding.ASCII);
            Buffer.BlockCopy(bs, 0, localbs[id].Item2, index * 2, bs.Length);
        }

        /// <summary>
        /// 从读取缓冲区读取
        /// </summary>
        /// <param name="index">字的索引</param>
        /// <param name="len">字符串长度</param>
        /// <returns></returns>
        public string GetString(int id, int index, int len)
        {
            return modbus.ByteTransform.TransString(localbs[id].Item1, index, len, Encoding.ASCII);

        }

        public bool GetSetBit(int id, int index, int bit)
        {
            return (modbus.ByteTransform.TransUInt16(localbs[id].Item2, index * 2) & (1 << bit)) > 0;
        }

        public void PlcConnect()
        {
            //  throw new NotImplementedException();
        }

    }
    class Program
    {
        static void Main(string[] args)
        {

            var plcscanner = new PlcScannerComm();
            var modbus = new ModbusDeviceReadWriter();
            plcscanner.SetString(0, 4, "LSC05130024Q1JF7C1EL".PadRight(20, '\0'));
            plcscanner.SetString(1, 4, "123456789A123456789A".PadRight(20, '\0'));
            plcscanner.SetString(2, 4, "123456789B123456789B".PadRight(20, '\0'));
            plcscanner.SetString(3, 4, "123456789C123456789C".PadRight(20, '\0'));
            Thread.Sleep(1000);
            var s1=  modbus.GetString(5 * 2, 20).Trim('\0');
            var s2 = modbus.GetString(5 * 2+40, 20).Trim('\0');
            var s3 = modbus.GetString(5 * 2+40*2, 20).Trim('\0');
            var s4 = modbus.GetString(5 * 2+40*3, 20).Trim('\0');
            Console.WriteLine(s1);
            Console.WriteLine(s2);
            Console.WriteLine(s3);
            Console.WriteLine(s4);
            Console.ReadLine();
            //try
            //{
            //    //  Console.WriteLine("Hello World!");
            //    Console.WriteLine("请输入URL,比如：http://10.13.250.249:8052/LTBAssemblyWebService.asmx");
            //    string url = Console.ReadLine();
            //    Console.WriteLine("请输入方法名,比如：UPLOADCoilWinding");
            //    string procName = Console.ReadLine();
            //    Console.WriteLine("请输入json内容,比如：json:{}");
            //    string json = Console.ReadLine();
            //    var vs = json.Split(':');

            //    var xmlDocument = WebSvcCaller.QuerySoapWebService(url, procName, new Hashtable() { { vs[0], vs[1] } });
            //    var value = xmlDocument.InnerText;
            //    Console.WriteLine(value);
            //    Console.ReadLine();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message+Environment.NewLine+ex.StackTrace);
            //    Console.ReadLine();
            //    // throw;
            //}
        }
    }

    public class WebSvcCaller
    {
        //<webServices>
        //  <protocols>
        //    <add name="HttpGet"/>
        //    <add name="HttpPost"/>
        //  </protocols>
        //</webServices>

        private static Hashtable _xmlNamespaces = new Hashtable();//缓存xmlNamespace，避免重复调用GetNamespace

        /**//// <summary>
            /// 需要WebService支持Post调用
            /// </summary>
        public static XmlDocument QueryPostWebService(String URL, String MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            byte[] data = EncodePars(Pars);
            WriteRequestData(request, data);

            return ReadXmlResponse(request.GetResponse());
        }
        /**//// <summary>
            /// 需要WebService支持Get调用
            /// </summary>
        public static XmlDocument QueryGetWebService(String URL, String MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName + "?" + ParsToString(Pars));
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            return ReadXmlResponse(request.GetResponse());
        }



        /**//// <summary>
            /// 通用WebService调用(Soap),参数Pars为String类型的参数名、参数值
            /// </summary>
        public static XmlDocument QuerySoapWebService(String URL, String MethodName, Hashtable Pars)
        {
            if (_xmlNamespaces.ContainsKey(URL))
            {
                return QuerySoapWebService(URL, MethodName, Pars, _xmlNamespaces[URL].ToString());
            }
            else
            {
                return QuerySoapWebService(URL, MethodName, Pars, GetNamespace(URL));
            }
        }


        private static XmlDocument QuerySoapWebService(String URL, String MethodName, Hashtable Pars, string XmlNs)
        {
            _xmlNamespaces[URL] = XmlNs;//加入缓存，提高效率
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "\"" + XmlNs + (XmlNs.EndsWith("/") ? "" : "/") + MethodName + "\"");
            SetWebRequest(request);
            byte[] data = EncodeParsToSoap(Pars, XmlNs, MethodName);
            WriteRequestData(request, data);
            XmlDocument doc = new XmlDocument(), doc2 = new XmlDocument();
            doc = ReadXmlResponse(request.GetResponse());
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            String RetXml = doc.SelectSingleNode("//soap:Body/*/*", mgr).InnerXml;
            doc2.LoadXml("<root>" + RetXml + "</root>");
            AddDelaration(doc2);
            return doc2;
        }
        private static string GetNamespace(String URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL + "?WSDL");
            SetWebRequest(request);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            sr.Close();
            return doc.SelectSingleNode("//@targetNamespace").Value;
        }
        private static byte[] EncodeParsToSoap(Hashtable Pars, String XmlNs, String MethodName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>");
            AddDelaration(doc);
            XmlElement soapBody = doc.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            XmlElement soapMethod = doc.CreateElement(MethodName);
            soapMethod.SetAttribute("xmlns", XmlNs);
            foreach (string k in Pars.Keys)
            {
                XmlElement soapPar = doc.CreateElement(k);
                soapPar.InnerXml = ObjectToSoapXml(Pars[k]);
                soapMethod.AppendChild(soapPar);
            }
            soapBody.AppendChild(soapMethod);
            doc.DocumentElement.AppendChild(soapBody);
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }
        private static string ObjectToSoapXml(object o)
        {
            XmlSerializer mySerializer = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            mySerializer.Serialize(ms, o);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            if (doc.DocumentElement != null)
            {
                return doc.DocumentElement.InnerXml;
            }
            else
            {
                return o.ToString();
            }
        }
        private static void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }


        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }


        private static byte[] EncodePars(Hashtable Pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(Pars));
        }


        private static String ParsToString(Hashtable Pars)
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


        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }


        private static void AddDelaration(XmlDocument doc)
        {
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.InsertBefore(decl, doc.DocumentElement);
        }
    }
}
