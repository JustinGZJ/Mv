
using MotionWrapper;
using Mv.Core;
using Mv.Modules.Axis.Views;
using Mv.Ui.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Mv.Modules.Axis
{
    public class AxisModule : IModule
    {
        private readonly IRegionManager regionManager;

        public AxisModule(IRegionManager _regionManager)
        {
            regionManager = _regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
          
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            regionManager.RegisterViewWithRegion("APPS", typeof(MainTab));
            regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(Setting));
            containerRegistry.RegisterSingleton<IConfigManager<MotionConfig>, Core.ConfigManager<MotionConfig>>();
        }
    }
}