namespace Mv.Modules.Axis.Controller
{
    public interface IMessageWraper
    {
        void Error(string msg, string source = "");
        void Msg(string msg, string source = "");
        void Warn(string msg, string source = "");
    }
}