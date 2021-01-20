
using MotionWrapper;
using Mv.Core;
using Mv.Core.Interfaces;
using Mv.Modules.Axis.Views;
using Mv.Modules.Axis.Controller;
using Mv.Ui.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;
using Unity;
using Mv.Modules.Axis.Controlers;

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
            var factory = containerProvider.Resolve<IFactory<string, ICylinder>>(nameof(CylinderFactory));
            var motion = containerProvider.Resolve<IGtsMotion>();
            if (!motion.Init())
            {
                MessageBox.Show("板卡初始化失败");
                return;
            }
            
            var manager = containerProvider.Resolve<MachineBase>(nameof(MachineManager));
            var loader = containerProvider.Resolve<MachineBase>(nameof(Loader));        
            var wielding = containerProvider.Resolve<MachineBase>(nameof(Wielding));
            loader.Home();
            wielding.Home();
            MachineManager machineManager = (manager as MachineManager);
            machineManager.AddMachine(loader);
            machineManager.AddMachine(wielding);

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            regionManager.RegisterViewWithRegion(RegionNames.AppsTabRegion, typeof(MainTab));
            regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(Setting));
            regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(Dashboard));
            containerRegistry.RegisterSingleton<IConfigManager<MotionConfig>, ConfigManager<MotionConfig>>();
            containerRegistry.RegisterSingleton<IGtsMotion, CGtsMotion>();
            containerRegistry.RegisterSingleton<IFactory<string, ICylinder>, CylinderFactory>(nameof(CylinderFactory));
            containerRegistry.RegisterSingleton<IMessageWraper, MessageWraper>();
            containerRegistry.RegisterSingleton<MachineBase, MachineManager>(nameof(MachineManager));
            containerRegistry.RegisterSingleton<MachineBase, Wielding>(nameof(Wielding));
            containerRegistry.RegisterSingleton<MachineBase, Loader>(nameof(Loader));
            containerRegistry.RegisterSingleton<IShareData, ShareData>();
            
        }
    }
}