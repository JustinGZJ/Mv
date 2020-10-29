using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.Axis.ViewModels.Dialog
{
    public class AxisSettingViewModel:BindableBase
    {
        public AxisSettingViewModel()
        {

        }
        private object selectedObject;
        public object SelectedObject
        {
            get { return selectedObject; }
            set { SetProperty(ref selectedObject, value); }
        }

    }
}
