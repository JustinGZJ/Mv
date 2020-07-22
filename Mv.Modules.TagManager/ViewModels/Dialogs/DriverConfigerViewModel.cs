using BatchCoreService;
using DataService;
using MaterialDesignThemes.Wpf;
using Mv.Modules.TagManager.Views.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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


        private DelegateCommand _addGroupCommand;
        public DelegateCommand AddGroupCommand =>
            _addGroupCommand ?? (_addGroupCommand = new DelegateCommand(async () => await AddGroupAsync()));

        private DelegateCommand<Group> delegateCommand;
        public DelegateCommand<Group> DelegateCommand =>
            delegateCommand ?? (delegateCommand = new DelegateCommand<Group>(group =>
            {
                Driver.Groups.Remove(group);
                RaisePropertyChanged();
            }));

        private async Task AddGroupAsync()
        {

            var dlg = new AddGroupDlg();
            var result = await DialogHost.Show(dlg, "RootDialog");
            if (result.ToString() == "OK")
            {
                var vm = (dlg.DataContext as AddGroupDlgViewModel);

                Driver.Groups.Add(new Group()
                {
                    DriverId = Driver.Id,
                    Name = vm.Name,
                    DeadBand = vm.DeadBand,
                    Active = vm.Active,
                    UpdateRate = vm.UpdateRate
                });

                this.RaisePropertyChanged();
            }
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
