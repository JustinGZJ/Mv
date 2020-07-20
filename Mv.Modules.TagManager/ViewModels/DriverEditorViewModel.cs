using BatchCoreService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Mv.Ui.Mvvm;
using Unity;
using Prism.Commands;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using Mv.Modules.TagManager.Views.Dialogs;
using Mv.Modules.TagManager.ViewModels.Dialogs;
using Mv.Modules.TagManager.Views;

namespace Mv.Modules.TagManager.ViewModels
{
    public class DriverEditerViewModel : ViewModelBase,IViewLoadedAndUnloadedAware<DriverEditer>
    {
        private readonly IDriverDataContext driverDataContext;

        public DriverEditerViewModel(IUnityContainer container, IDriverDataContext driverDataContext) : base(container)
        {
            this.driverDataContext = driverDataContext;
            var drivers = driverDataContext.GetDrivers();
            Drivers.AddRange(drivers);
        }

        public ObservableCollection<Driver> Drivers { get; set; } = new ObservableCollection<Driver>();

        private DelegateCommand _addDriverCommand;
        public DelegateCommand AddDriverCommand =>
            _addDriverCommand ?? (_addDriverCommand = new DelegateCommand(async ()=>await AddDriverAsync()));

        private DelegateCommand<Driver> _removeDriverCommand;
        public DelegateCommand<Driver> RemoveDriverCommand =>
            _removeDriverCommand ?? (_removeDriverCommand = new DelegateCommand<Driver>(RemoveDriver));

        private DelegateCommand<Driver> _showDriverCommand;
        public DelegateCommand<Driver> ShowDriverCommand =>
            _showDriverCommand ?? (_showDriverCommand = new DelegateCommand<Driver>(ShowDriver));


        private async Task AddDriverAsync()
        {
            var dlg = new AddDriverDlg();
            var result = await DialogHost.Show(dlg, "RootDialog");
            if(result.ToString()=="OK")
            {
                Drivers.Add((dlg.DataContext as AddDriverDlgViewModel).Driver);
            }
        }
        private void RemoveDriver(Driver driver)
        {
            Drivers.Remove(driver);
        }

        private void ShowDriver(Driver driver)
        {
            
        }

        void IViewLoadedAndUnloadedAware<DriverEditer>.OnLoaded(DriverEditer view)
        {
      //      throw new NotImplementedException();
        }

        void IViewLoadedAndUnloadedAware<DriverEditer>.OnUnloaded(DriverEditer view)
        {
            driverDataContext.SetDrivers(Drivers);
        }
    }
}
