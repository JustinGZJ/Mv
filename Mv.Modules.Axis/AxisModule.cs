
using MotionWrapper;
using Mv.Core;
using Mv.Modules.Axis.Views;
using Mv.Ui.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;
using Unity;

namespace Mv.Modules.Axis
{
    public class AxisModule : IModule
    {
        private readonly IRegionManager regionManager;

        public AxisModule(IUnityContainer unityContainer, IRegionManager _regionManager)
        {
            UnityContainer = unityContainer;
            regionManager = _regionManager;
        }

        public IUnityContainer UnityContainer { get; }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var motion = containerProvider.Resolve<IGtsMotion>();
            if (!motion.Init())
                MessageBox.Show("板卡初始化失败");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            regionManager.RegisterViewWithRegion("APPS", typeof(MainTab));
            regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(Setting));
            containerRegistry.RegisterSingleton<IConfigManager<MotionConfig>, ConfigManager<MotionConfig>>();
            containerRegistry.RegisterSingleton<IGtsMotion, CGtsMotion>();
        }
    }
}