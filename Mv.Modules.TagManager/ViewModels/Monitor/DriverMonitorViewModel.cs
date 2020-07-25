using BatchCoreService;
using DataService;
using Mv.Modules.TagManager.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Mv.Modules.TagManager.ViewModels
{
   public class DriverMonitorViewModel
    {
        public IDataServer DataServer { get; set; }
        public ObservableCollection<IDriver> Drivers => new ObservableCollection<IDriver>(DataServer.Drivers.ToArray());

        public DriverMonitorViewModel(IDataServer server,IRegionManager regionManager)
        {
            this.DataServer = server;
            this.regionManager = regionManager;
        }

        private DelegateCommand<IDriver> _showGroupsCommand;
        private readonly IRegionManager regionManager;

        public DelegateCommand<IDriver> ShowGroupsCommand =>
            _showGroupsCommand ?? (_showGroupsCommand = new DelegateCommand<IDriver>(ShowGroup));

        private void ShowGroup(IDriver driver)
        {
            if (driver == null)
                return;
            regionManager.RequestNavigate("MONITOR_DETAIL", nameof(GroupMonitor), new NavigationParameters
            {
                { nameof(IDriver), driver }
            });

        }


    }
}
