using MotionWrapper;
using Mv.Core;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.Axis.ViewModels
{
    public class SettingViewModel : BindableBase
    {
        object selectedObject;
        private readonly IConfigManager<MotionConfig> configManager;

        public object SelectedObject
        {
            get => selectedObject; set { SetProperty(ref selectedObject, value); }


        }
        public SettingViewModel(IConfigManager<MotionConfig> configManager)
        {
            this.configManager = configManager;
            SelectedObject = configManager.Get();
        }
    }
}

