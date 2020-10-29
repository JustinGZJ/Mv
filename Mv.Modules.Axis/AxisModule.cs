
using MotionWrapper;
using Mv.Core;
using Mv.Modules.Axis.Views;
using Mv.Ui.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;

namespace Mv.Modules.Axis
{
    public class AxisModule : IModule
    {
        private readonly IRegionManager regionManager;

        public AxisModule(IUnityContainer unityContainer,IRegionManager _regionManager)
        {
            UnityContainer = unityContainer;
            regionManager = _regionManager;
        }

        public IUnityContainer UnityContainer { get; }

        public void OnInitialized(IContainerProvider containerProvider)
        {
          
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            regionManager.RegisterViewWithRegion("APPS", typeof(MainTab));
            regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(Setting));
            containerRegistry.RegisterSingleton<IConfigManager<MotionConfig>,ConfigManager<MotionConfig>>();
            containerRegistry.RegisterSingleton<IGtsMotion, CGtsMotion>();            
        }
    }
}