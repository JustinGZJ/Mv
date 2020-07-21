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
using DataService;
using System.Reflection;
using System.Linq;
using System.Globalization;
using PropertyTools.Wpf;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace Mv.Modules.TagManager.ViewModels
{
    public class DriverEditerViewModel : ViewModelBase, IViewLoadedAndUnloadedAware<DriverEditer>
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
            _addDriverCommand ?? (_addDriverCommand = new DelegateCommand(async () => await AddDriverAsync()));

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
            if (result.ToString() == "OK")
            {
                Drivers.Add((dlg.DataContext as AddDriverDlgViewModel).Driver);
            }
        }
        private void RemoveDriver(Driver driver)
        {
            Drivers.Remove(driver);
        }

        private IDriver selectPropery;
        public IDriver SelectPropery
        {
            get { return selectPropery; }
            set { SetProperty(ref selectPropery, value); }
        }

        private void ShowDriver(Driver driver)
        {
            IDriver dv = null;
            try
            {
                Assembly ass = Assembly.LoadFrom(driver.Assembly);
                var dvType = ass.GetType(driver.ClassName);

                if (dvType != null)
                {
                    dv = Activator.CreateInstance(dvType,
                        new object[] { null, driver.Id, driver.Name, string.IsNullOrEmpty(driver.Server) ? "127.0.0.1" : driver.Server, driver.Timeout == 0 ? 500 : driver.Timeout, driver.Arguments.ToDictionary(x => x.PropertyName, x => x.PropertyValue) }) as IDriver;

                    var paras = driver.Arguments.ToDictionary(x => x.PropertyName, x => x.PropertyValue);
                    var properties = dvType.GetProperties().Where(x => x.CanWrite).Where(x => paras.Keys.Contains(x.Name));
                    foreach (var para in paras)
                    {
                        var prop = properties.FirstOrDefault(x => x.Name == para.Key);
                        if (prop != null)
                        {
                            if (prop.PropertyType.IsEnum)
                                prop.SetValue(dv, Enum.Parse(prop.PropertyType, para.Value), null);
                            else
                                prop.SetValue(dv, Convert.ChangeType(para.Value, prop.PropertyType, CultureInfo.CreateSpecificCulture("en-US")), null);
                        }
                    }
                    SelectPropery = dv;
                    var dialog = new PropertyDialog()
                    {
                        DataContext = SelectPropery,
                        Title = "设置驱动"
                    };
                    dialog.ShowDialog();
                }
            }
            catch (Exception e)
            {
                //    AddErrorLog(e);
            }
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
