using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MotionWrapper
{
    /// <summary>
    /// 用来编译G代码或者其他代码的工具
    /// </summary>
    public class CompileUserCode
    {
        string regexFloat = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";
        string regexLong = "(\\d+)";
        public string getFloatOrLong(string key, string cmd)
        {
            //先测试浮点数
            string tmpStr = Regex.Match(cmd, "(" + key + ")" + regexFloat).Value;
            if (tmpStr != "")
            {
                return Regex.Match(tmpStr, regexFloat).Value;
            }
            else
            {
                //测试LONG
                tmpStr = Regex.Match(cmd, "(" + key + ")" + regexLong).Value;
                if (tmpStr != "")
                {
                    return Regex.Match(tmpStr, regexLong).Value;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
