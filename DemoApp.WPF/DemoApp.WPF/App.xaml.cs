using DemoApp.WPF.Modules;
using DemoApp.WPF.Services.Interfaces;
using DemoApp.WPF.Services;
using DemoApp.WPF.ViewModels;
using DemoApp.WPF.Views;
using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace DemoApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static IConfigurationRoot Configuration { get; private set; }
        public static bool IsDesignTime { get; private set; } = true;


        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
            .Register<IManageWorkers, ManageWorkersRPC>()
            .Register<IReport, XlsReport>()
            .RegisterDialog<WorkerEditDialog, WorkerEditDialogViewModel>("EditWorkerDialog");
            containerRegistry.RegisterDialog<ChildEditDialog, ChildEditDialogViewModel>("EditChildDialog")
            ;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>();
        }

        public App()
        {
            IsDesignTime = false;
            Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();


        }
    }
}
