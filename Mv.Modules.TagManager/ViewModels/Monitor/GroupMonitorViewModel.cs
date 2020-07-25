using DataService;
using Mv.Modules.TagManager.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Mv.Modules.TagManager.ViewModels
{
    public class GroupMonitorViewModel : BindableBase, INavigationAware
    {

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }


        public ObservableCollection<TagItem> TagItems { get; set; } = new ObservableCollection<TagItem>();
        public IDriver Driver { get; set; }

        public ObservableCollection<IGroup> Groups => Driver != null ? new ObservableCollection<IGroup>(Driver.Groups) : null;

        private DelegateCommand<IGroup> _selectedChanged;
        public DelegateCommand<IGroup> SelectedChanged =>
            _selectedChanged ?? (_selectedChanged = new DelegateCommand<IGroup>(ExecuteSelectedChanged));

        void ExecuteSelectedChanged(IGroup group)
        {
            TagItems.Clear();
            foreach (var item in group.Items)
            {
                TagItems.Add(new TagItem(item, item.GetTagName(), item.GetMetaData().Address));
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            if (navigationContext.Parameters[nameof(IDriver)] is IDriver dv)
            {
                if (dv != null)
                {
                    Driver = dv;
                    this.RaisePropertyChanged(nameof(Groups));
                }
            }
        }
        public TagItem SelectedItem { get; set; }
       
    }
}
