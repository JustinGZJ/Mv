using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MotionWrapper;
using Mv.Core;
using Unity;

namespace Mv.Modules.Axis.ViewModels
{
    public class AxisesControlViewModel : BindableBase
    {

        public AxisesControlViewModel(IUnityContainer container)
        {
            var motion = container.Resolve<IGtsMotion>();
            var axisRefs = motion.AxisRefs.Where(x => x.Name != "").ToList();
            AxisRefs = axisRefs.Select(x => new SingleAxisViewModel(container)
            {
                SelectedAxisRef = x,
                Selectable = false
            }).ToList();
        }

        public List<SingleAxisViewModel> AxisRefs { get; }
    }
}
