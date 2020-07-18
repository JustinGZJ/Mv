using BatchCoreService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mv.Modules.TagManager.ViewModels
{
    public class DriverEditorViewModel
    {
        private readonly IDriverDataContext driverDataContext;

        public DriverEditorViewModel(IDriverDataContext driverDataContext)
        {
            this.driverDataContext = driverDataContext;
            var drivers=driverDataContext.GetDrivers();
            Drivers.AddRange(drivers);          
        }

        public ObservableCollection<Driver> Drivers { get; set; } = new ObservableCollection<Driver>();
    }
}
