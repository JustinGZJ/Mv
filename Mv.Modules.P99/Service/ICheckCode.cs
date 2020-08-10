using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Mv.Modules.P99.Service
{
    public interface ICheckCode
    {
        string CheckPass(string code, string station);
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
