using BatchCoreService;
using DataService;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.TagManager.ViewModels.Dialogs
{
    public class DriverConfigerViewModel : BindableBase, INavigationAware
    {
        public DriverConfigerViewModel()
        {

        }
        private Driver driver;
        public Driver Driver
        {
            get { return driver; }
            set { SetProperty(ref driver, value); }
        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ///hrow new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters[nameof(Driver)] is Driver dv)
            {
                if (dv != null)
                {
                    Driver = dv;
                    return true;
                }
            }
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
