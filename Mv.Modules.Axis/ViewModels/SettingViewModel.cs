using MotionWrapper;
using Mv.Core;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mv.Modules.Axis.ViewModels
{
    public class SettingViewModel : BindableBase
    {
        object selectedObject;
        private readonly IConfigManager<MotionConfig> configManager;

        public IEnumerable<object> Collection
        { get; }


        public object SelectedObject
        {
            get => selectedObject; set { SetProperty(ref selectedObject, value); }
        }
        public SettingViewModel(IConfigManager<MotionConfig> configManager)
        {
            this.configManager = configManager;
          var  v = configManager.Get();
            var m = v.GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType)
                .Where(p => p.PropertyType.GetInterface("IEnumerable", false) != null)
                .Where(p => p.PropertyType.GetGenericArguments()[0] != typeof(char));
            Collection = m.Select(x => new
            {
                Key = x.Name,
                Value = x.GetValue(v)
            }); 
        }
    }
}

