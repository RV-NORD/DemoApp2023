using DemoApp.WPF.Core;
using DemoApp.WPF.Modules.WorkerModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace DemoApp.WPF.Modules.WorkerModule
{
    public class WorkerModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public WorkerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "ViewA");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();
        }
    }
}