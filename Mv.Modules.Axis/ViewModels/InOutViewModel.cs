using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MotionWrapper;
using Unity;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Reactive.Linq;

namespace Mv.Modules.Axis.ViewModels
{
    public class InOutViewModel : BindableBase
    {
        private readonly ICMotionData data;


        public InOutViewModel(IUnityContainer container)
        {
            data = container.Resolve<IGtsMotion>();
            IIoPart1 part1 = container.Resolve<IGtsMotion>();
            Observable.Interval(TimeSpan.FromMilliseconds(100)).ObserveOnDispatcher().Subscribe(x =>
            {
                foreach (var item in data.Dos.Where(x => !string.IsNullOrEmpty(x.Name)))
                {
                    item.Value = part1.getDi(item);
                }
                foreach (var item in data.Dis.Where(x => !string.IsNullOrEmpty(x.Name)))
                {
                    item.Value = part1.getDo(item);
                }
            });

        }
        public ObservableCollection<IoRef> DO => new ObservableCollection<IoRef>(data.Dos.Where(x => !string.IsNullOrEmpty(x.Name)));
        public ObservableCollection<IoRef> DI => new ObservableCollection<IoRef>(data.Dis.Where(x => !string.IsNullOrEmpty(x.Name)));
    }
}
