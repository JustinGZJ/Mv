using PropertyChanged;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MotionWrapper
{
    public interface ICylinder
    {
        TimeSpan TimeOut { get; set; }
        IoRef[] Output { get; }
        IoRef[] Input { get; }

        public bool State { get;  }

        Task<bool> Set();
        Task<bool> Reset();
    }
    [AddINotifyPropertyChangedInterface]
    public class Cylinder : ICylinder
    {
        private readonly IIoPart1 _ioPart1;
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(2);

        public IoRef[] Output { get; }
        public IoRef[] Input { get; }

        public bool State => Output[0].Value;

        public Cylinder(IoRef[] output, IoRef[] input, IIoPart1 ioPart1)
        {
            Output = output;
            Input = input;
            _ioPart1 = ioPart1;

        }

        public Task<bool> Set()
        {
            _ioPart1.setDO(Output[0], true);
            return Task.Run(() => (SpinWait.SpinUntil(() => (!Input[0].Value) && (Input[1].Value), TimeOut)));
        }

        public Task<bool> Reset()
        {
            _ioPart1.setDO(Output[0], false);
            return Task.Run(() => (SpinWait.SpinUntil(() => Input[0].Value && (!Input[1].Value), TimeOut)));

        }
    }

    [AddINotifyPropertyChangedInterface]
    public class OneSignalCylinder : ICylinder
    {
        private readonly IIoPart1 _ioPart1;
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(2);

        public IoRef[] Output { get; }
        public IoRef[] Input { get; }

        public bool State => Output[0].Value;

        public OneSignalCylinder(IoRef[] output, IoRef[] input, IIoPart1 ioPart1)
        {
            Output = output;
            Input = input;
            _ioPart1 = ioPart1;
        }

        public Task<bool> Set()
        {
            _ioPart1.setDO(Output[0], true);
            return Task.Run(() => (SpinWait.SpinUntil(() => (Input[0].Value), TimeOut)));
        }

        public Task<bool> Reset()
        {
            _ioPart1.setDO(Output[0], false);
            return Task.Run(() => (SpinWait.SpinUntil(() => (!Input[0].Value), TimeOut)));
        }
    }
}