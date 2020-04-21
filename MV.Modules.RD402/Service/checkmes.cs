using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections;
using System.Data.SqlClient;

namespace Mv.Modules.RD402.Service
{
    public class checkmes : IGetSn
    {
        private string strConnection;

        private SqlConnection conn;

        public checkmes()
        {
            strConnection = "data source=KSSFClisa.luxshare.com.cn;initial catalog=MESDB; user id=dataquery; password=querydata";
            conn = new SqlConnection(strConnection);
        }

        public string checkAAB(string sn, string stationid, string stationname)
        {
            conn.Open();
            string text = "";
            int num = 0;
            string cmdText = "select ppid from m_testresult_t with (nolock) where ppid='" + sn + "' and stationid='" + stationid + "'";
            SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Close();
                cmdText = "select len(replace(teststationid,'" + stationname + "','" + stationname + "-'))-len(teststationid) as count  from m_testresult_t with (nolock) where ppid='" + sn + "' and stationid='" + stationid + "'";
                sqlCommand = new SqlCommand(cmdText, conn);
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    num = Conversions.ToInteger(Conversion.Int(sqlDataReader["count"].ToString()));
                }
                text = ((num < 2) ? "PASS" : "FAIL");
            }
            else
            {
                text = "PASS";
            }
            sqlDataReader.Close();
            conn.Close();
            return text;
        }

        public string checkroute(string sn, string stationid)
        {
            conn.Open();
            string text = "";
            string cmdText = "select ppid from m_testresult_t with (nolock) where ppid='" + sn + "' and stationid='" + stationid + "'";
            SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            text = ((!sqlDataReader.HasRows) ? "FAIL" : "PASS");
            sqlDataReader.Close();
            conn.Close();
            return text;
        }

        public string checklink(string sn)
        {
            conn.Open();
            string text = "";
            string cmdText = "select ppid from m_assysnd_t with (nolock) where  Stationid='A00055' and Estateid='Y' and  ppid='" + sn + "'";
            SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            text = ((!sqlDataReader.HasRows) ? "FAIL" : "PASS");
            sqlDataReader.Close();
            conn.Close();
            return text;
        }

        public string Querysn(string sn)
        {
            conn.Open();
            string result = "";
            string cmdText = "select exppid from m_ppidlink_t  with (nolock) where  StationId='A00066' and ppid='" + sn + "'";
            SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    result = sqlDataReader["exppid"].ToString();
                }
            }
            else
            {
                result = "FAIL";
            }
            sqlDataReader.Close();
            conn.Close();
            return result;
        }

        public (bool,string) getsn(Hashtable hashtable)
        {
            try
            {
                var moid = hashtable["moName"].ToString();
                var line = hashtable["lineNumber"].ToString();
                conn.Open();
                string result = "FAIL";
                string cmdText = "DECLARE @moid varchar(50)='" + moid + "',@teamid varchar(50)='" + line + "',@strmsgid varchar(1) ,@strmsgText nvarchar(100),@FSN nvarchar(50) EXECUTE [dbo].[m_AutoGenP33FSN_P]   @moid,@teamid,@strmsgid output,@strmsgText output,@FSN output  select @strmsgid as strmsgid ,@strmsgText as strmsgText, @FSN as FSN";
                SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        result = sqlDataReader["strmsgid"].ToString() + ";" + sqlDataReader["strmsgText"].ToString() + ";" + sqlDataReader["FSN"].ToString() + ";";
                    }
                }
                else
                {
                    result = "FAIL";
                }
                sqlDataReader.Close();
                conn.Close();
                return result.Contains("已獲取到") ? (true, result.Split(';')[2]) : (false,result);
            }
            catch (System.Exception ex)
            {
                return (false,ex.Message);
            }         
        }

        public string W1Querysn(string sn, string position)
        {
            conn.Open();
            string result = "";
            string cmdText = "select exppid from m_ppidlink_t  with (nolock) where  StationId='A00061' and  codeid= '" + position + "' and ppid='" + sn + "'";
            SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    result = sqlDataReader["exppid"].ToString();
                }
            }
            else
            {
                result = "FAIL";
            }
            sqlDataReader.Close();
            conn.Close();
            return result;
        }

        public string InputSN(string ppid, string moid, string line)
        {
            conn.Open();
            string text = "";
            string left = "";
            string text2 = "";
            string cmdText = "declare @strmsgid varchar(70),@strmsgText varchar(70) set @strmsgid='' set @strmsgText=''  execute MESDB.[dbo].[m_CheckP60AssemblyPPIDA00070_P] '" + ppid + "','" + moid + "','" + line + "',@strmsgid output ,@strmsgText output select @strmsgid as strmsgid,@strmsgText as strmsgText";
            SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    left = sqlDataReader["strmsgid"].ToString();
                    text2 = sqlDataReader["strmsgText"].ToString();
                }
            }
            text = ((Operators.CompareString(left, "0", TextCompare: false) != 0) ? text2 : "PASS");
            sqlDataReader.Close();
            conn.Close();
            return text;
        }
    }
}
