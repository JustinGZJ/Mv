using Mv.Modules.Group.Views;
using Mv.Ui.Core;
using Mv.Ui.Core.Modularity;
using Prism.Ioc;
using Prism.Regions;
using Unity;

namespace Mv.Modules.Group
{
    public class GroupModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public GroupModule(IUnityContainer container, IRegionManager regionManager) : base(container)
        {
            _regionManager = regionManager;
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(GroupComponent));
            _regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(GroupSettingsTabItem));

        }
    }
}