using MotionWrapper;
using Mv.Core.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mv.Modules.Axis.ViewModels
{
    public class CylinderManagerViewModel : BindableBase
    {
        MotionConfig cfg;
        public CylinderManagerViewModel(IConfigureFile config)
        {
            Cfg = config.GetValue<MotionConfig>(nameof(MotionConfig));
            
 
        }
        public MotionConfig Cfg { get => cfg; set => cfg = value; }
    }
}
