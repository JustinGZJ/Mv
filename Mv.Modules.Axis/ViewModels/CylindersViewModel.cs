using MotionWrapper;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using Unity;

namespace Mv.Modules.Axis.ViewModels
{
    public class CylindersViewModel:BindableBase
    {
        public CylindersViewModel(IUnityContainer container)
        {
            Cylinders = container.ResolveAll<ICylinder>();
        }

        public IEnumerable<ICylinder> Cylinders { get;  }

        private DelegateCommand<ICylinder> cmdReverse;
        public DelegateCommand<ICylinder> CmdReverse =>
            cmdReverse ??= new DelegateCommand<ICylinder>(ExecuteCmdReverse, CanExecuteCmdReverse);

        void ExecuteCmdReverse(ICylinder cylinder)
        {
            if (!cylinder.State)
            {
                cylinder.Set();
            }
            else
            {
                cylinder.Reset();
            }
        }

        bool CanExecuteCmdReverse(ICylinder cylinder)
        {
            return true;
        }
    }

}