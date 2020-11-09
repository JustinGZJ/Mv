using System.ComponentModel;
using Mv.Ui.Core;

namespace MotionWrapper
{
    /// <summary>
    /// IO的类型
    /// </summary>
    ///
     [TypeConverter(typeof(EnumDefaultValueTypeConverter))]
    public enum EIoType
    {
        [Description("通用输入"),DefaultValue("通用输入")]
        NoamlInput = 1,
        [Description("通用输出"),DefaultValue("通用输出")]
        NomalOutput = 9,
        [Description("报警"),DefaultValue("报警")]
        Alarm = 2,
        [Description("正限位"),DefaultValue("正限位")]
        LimitP = 3,
        [Description("负限位"),DefaultValue("负限位")]
        LimitN = 4,
        [Description("回零"),DefaultValue("回零")]
        Home = 5,
        [Description("到位"),DefaultValue("到位")]
        Arrive = 6,
        [Description("伺服使能"),DefaultValue("伺服使能")]
        ServoOn = 7,
        [Description("清除状态"),DefaultValue("清除状态")]
        clrSts = 8,

    }
}