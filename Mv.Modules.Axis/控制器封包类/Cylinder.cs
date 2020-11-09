using System;
using System.Threading;
using System.Threading.Tasks;

namespace MotionWrapper
{
    public interface ICylinder
    {
        TimeSpan TimeOut { get; set; }
        Task<bool> Set();
        Task<bool> Reset();
    }

    public class Cylinder : ICylinder
    {
        private readonly IoRef _output;
        private readonly IoRef _input;
        private readonly IIoPart1 _ioPart1;
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(2);

        public Cylinder(IoRef output,IoRef input,IIoPart1 ioPart1)
        {
            _output = output;
            _input = input;
            _ioPart1 = ioPart1;
        }

        public Task<bool> Set()
        {
            _ioPart1.setDO(_output,true);
            return Task.Run(() => (SpinWait.SpinUntil(() => _input.Value, TimeOut)));
        }

        public Task<bool> Reset()
        {
            _ioPart1.setDO(_output,false);
            return Task.Run(() => (SpinWait.SpinUntil(() => !_input.Value, TimeOut)));         
        }
    }
}