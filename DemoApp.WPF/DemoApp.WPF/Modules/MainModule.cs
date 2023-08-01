using DemoApp.WPF.Services.Interfaces;
using DemoApp.WPF.Services;
using DemoApp.WPF.ViewModels;
using DemoApp.WPF.Views;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using DemoApp.WPF.Core;

namespace DemoApp.WPF.Modules
{
    class MainModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MainModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("PopulateRegion", typeof(PopulateView))
                .RegisterViewWithRegion("StatRegion", typeof(StatView))
                .RegisterViewWithRegion("WorkerRegion", typeof(WorkerView))
                ;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
