using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;

namespace MotionWrapper
{
    [AddINotifyPropertyChangedInterface]
    public class ABCylinder : ICylinder
    {
        private readonly IoRef[] _output;
        private readonly IoRef[] _input;
        private readonly IIoPart1 _ioPart1;

        public ABCylinder(IoRef[] output, IoRef[] input, IIoPart1 ioPart1)
        {
            if (output == null || input == null || ioPart1 == null)
                throw new ArgumentNullException();
            if (output.Length < 2 || input.Length > 2)
                throw new ArgumentOutOfRangeException("IO所需参数不够");
            _output = output;
            _input = input;
            _ioPart1 = ioPart1;
        }
        


        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(2);

        public IoRef[] Output => _output;

        public IoRef[] Input => _input;

        public async Task<bool> Set()
        {
            _ioPart1.setDO(Output[0], true);
            _ioPart1.setDO(Output[1], false);
            var m1 = Task.Run(() => SpinWait.SpinUntil(() => Input[0].Value, TimeOut));
            var m2 = Task.Run(() => SpinWait.SpinUntil(() => !Input[1].Value, TimeOut));
            var results = await Task.WhenAll(m1, m2);
            return results.All(x => x);
        }

        public async Task<bool> Reset()
        {
            _ioPart1.setDO(Output[0], false);
            _ioPart1.setDO(Output[1], true);
            var m1 = Task.Run(() => SpinWait.SpinUntil(() => !Input[0].Value, TimeOut));
            var m2 = Task.Run(() => SpinWait.SpinUntil(() => Input[1].Value, TimeOut));
            var results = await Task.WhenAll(m1, m2);
            return results.All(x => x);
        }
    }
}