using BatchCoreService;
using DataService;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.TagManager.ViewModels.Dialogs
{
    public class DriverConfigerViewModel: BindableBase
    {
        public DriverConfigerViewModel()
        {

        }

        private IDriver selectPropery;
        public IDriver SelectPropery
        {
            get { return selectPropery; }
            set { SetProperty(ref selectPropery, value); }
        }
        private Driver driver;
        public Driver Driver
        {
            get { return driver; }
            set { SetProperty(ref driver, value); }
        }
    }
}
