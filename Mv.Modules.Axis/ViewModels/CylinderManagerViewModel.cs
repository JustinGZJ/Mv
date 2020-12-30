using MotionWrapper;
using Mv.Core;
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
        private readonly IConfigManager<MotionConfig> config;

        public CylinderManagerViewModel(IConfigManager<MotionConfig> config)
        {
            Cfg = config.Get();
            this.config = config;
        }
        private MotionConfig cfg;
        public MotionConfig Cfg
        {
            get { return cfg; }
            set { SetProperty(ref cfg, value); }
        }
    }
}
