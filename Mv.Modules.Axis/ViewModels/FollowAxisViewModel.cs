using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using MotionWrapper;

namespace Mv.Modules.Axis.ViewModels
{
    public class FollowAxisViewModel : BindableBase
    {
        private readonly IGtsMotion motion;

        public SingleAxisViewModel MasterAxis { get; set; }
        public SingleAxisViewModel SlaveAxis { get; set; }
        public FollowAxisViewModel(IUnityContainer unityContainer,IGtsMotion motion)
        {

            MasterAxis = new SingleAxisViewModel(unityContainer)
            {
                SelectedAxisRef = motion.AxisRefs.FirstOrDefault(x => x.Name == "SP")
            };
            SlaveAxis = new SingleAxisViewModel(unityContainer)
            {
                SelectedAxisRef = motion.AxisRefs.FirstOrDefault(x => x.Name == "Y")
            };
            this.motion = motion;
           
        }
    }
}
