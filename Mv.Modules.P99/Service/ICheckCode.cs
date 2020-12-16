using Prism.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Mv.Core.Interfaces;

namespace Mv.Modules.P99.Service
{

    public interface ICheckCode
    {
        string CheckPass(string code, string station);
    }
    public interface IUpload
    {
        string Upload(string code, Dictionary<string, string> dic);
    }

    public class LinYiUpload : IUpload
    {
        public LinYiUpload(ILoggerFacade logger, IConfigureFile configureFile)
        {
            this.logger = logger;
            this.configureFile = configureFile;
        }
        public const string urlbase = "http://10.13.250.249:8052/LTBAssemblyWebService.asmx";
        private readonly ILoggerFacade logger;
        private readonly IConfigureFile configureFile;

        public string Upload(string code, Dictionary<string, string> dic)
        {
            try
            {
                P99Config p99Config = configureFile.GetValue<P99Config>(nameof(P99Config));
                var hash = new Hashtable()
                {
                    ["json"] = new UploadData
                    {
                        Project = "LTBWhiPlash",
                        Sn = code,
                        SpindleNo = dic["Spindle NO."],
                        Station = p99Config.Station,
                        Result = dic["Result"],
                        MandrelNo = dic["Mandrel NO."],
                        Line = p99Config.LineNo
                    }.ToJson()
                };
                logger.Log("Upload POST:" + hash["json"].ToString(), Category.Debug, Priority.None);
                var result = WebSvcCaller.QuerySoapWebService(urlbase, "UPLOADCoilWinding", hash);
             //   var result = Utils.PostHelper.Post(urlbase + @"/UPLOADCoilWinding", hash);
                logger.Log("Upload RESULT:" + result.InnerText, Category.Debug, Priority.None);
                if (bool.TryParse(result.InnerText, out bool value))
                {
                    return value ? "PASS" : "FAIL";
                }
                else
                {
                    return "ERROR CONTENT";
                }
            }
            catch (Exception EX)
            {

                logger.Log(EX.Message + "\n" + EX.StackTrace, Category.Exception, Priority.High);
                return EX.Message;
            }
        }
    }


    public class LinYiCheckCode : ICheckCode
    {
        public LinYiCheckCode(ILoggerFacade logger)
        {
            this.logger = logger;
        }
        public const string urlbase = "http://10.13.250.249:8052/LTBAssemblyWebService.asmx";
        private readonly ILoggerFacade logger;

        Dictionary<int, string> keyValues = new Dictionary<int, string>()
        {
            { 0,"前道工站数据监测结果不符合当前工站的设定结果" },
            { 1,"验证通过PASS" },
            { 2,"json反序列化异常" },
            { 3,"检测时异常" },
            { 4,"前道工站无产品信息" },
            { 5,"无项目和工序的Routing信息" },
            {6,"产品信息在当前工站上传次数超出维护标准" }
        };

        public string CheckPass(string code, string station)
        {

            try
            {
                var hash = new Hashtable()
                {
                    ["json"] = new CheckData { Project = "LTBWhiPlash", Sn = code, Station = station }.ToJson()
                };
                logger.Log("CheckFormData POST:" + hash["json"].ToString(), Category.Debug, Priority.None);
                // var res = Utils.PostHelper.Post(urlbase + @"/CheckFormData", hash);
                var res = WebSvcCaller.QuerySoapWebService(urlbase, "CheckFormData", hash);
                logger.Log("CheckFormData RECIEVE:" + res.InnerText, Category.Debug, Priority.None);
                if (int.TryParse(res.InnerText, out int value))
                {
                    return keyValues[value];
                }
                else
                {
                    return res.InnerText;
                }
            }
            catch (Exception ex)
            {
                return "POST ERROR:" + ex.Message;
            }
        }
    }

    public class ICTCheckCode : ICheckCode
    {
        SqlConnectionStringBuilder stringBuilder;
        const string strConnection = "data source=KSSFClisa.luxshare.com.cn;initial catalog=MESDB; user id=dataquery; password=querydata;";
        public ICTCheckCode()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(strConnection)
            {
                ConnectTimeout = 2,
                Pooling = true,
                MinPoolSize = 4,
                MaxPoolSize = 100,
            };
            stringBuilder = sqlConnectionStringBuilder;

        }
        public string CheckPass(string code, string station)
        {
            string result = "";
            using (var con = new SqlConnection(stringBuilder.ToString()))
            {
                con.Open();
                var strSql = "select result from m_testresult_t where ppid='" + code + "' and stationid='" + station + "' ";
                SqlDataReader DataReader = new SqlCommand(strSql, con).ExecuteReader();
                if (DataReader.HasRows)
                {
                    while (DataReader.Read())
                    {
                        result = DataReader["result"].ToString();
                    }
                }
            }
            return result;
        }
    }
}
