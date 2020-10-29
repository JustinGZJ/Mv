using MotionWrapper;
using Mv.Core;
using Mv.Modules.Axis.Views;
using Mv.Ui.Mvvm;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mv.Modules.Axis.ViewModels
{
    public class SettingViewModel : BindableBase,IViewLoadedAndUnloadedAware<Setting>
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
        
        }

        public void OnLoaded(Setting view)
        {
            SelectedObject = configManager.Get();
        }

        public void OnUnloaded(Setting view)
        {
            configManager.Set(SelectedObject as MotionConfig);
        }
    }
}

