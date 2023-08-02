using DemoApp.WPF.Core.Messages;
using DemoApp.WPF.Services.Interfaces;
using DemoApp.WPF.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DemoApp.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Демонстрационное приложение PRISM MVVM";
        private readonly IManageWorkers _Service;
        private IEventAggregator _EA;
        private IRegionManager _RegionManager;
        CancellationTokenSource cts = new CancellationTokenSource();

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _StatusMsg;
        public string StatusMsg
        {
            get { return _StatusMsg; }
            set { SetProperty(ref _StatusMsg, value); }
        }

        private bool _DBOK = false;
        public bool DBOK
        {
            get { return _DBOK; }
            set { SetProperty(ref _DBOK, value); }
        }

        


        private DelegateCommand _LoadDataCommand;
        public DelegateCommand LoadDataCommand =>
            _LoadDataCommand ?? (_LoadDataCommand = new DelegateCommand(ExecuteLoadDataCommand));

        async void ExecuteLoadDataCommand()
        {
            CancellationToken ct = cts.Token;
            StatusMsg = "попытка подключения к БД";
            do
            {
                StatusMsg = "повторная попытка подключения к БД";
                if (await _Service.IsDBActiveAsync(ct))
                {
                    await _Service.ReCreateDB();
                    DBOK = true;
                    StatusMsg = "БД подключено";
                    _EA.GetEvent<DbConnectMsg>().Publish("DBOK");

                }
                else
                {
                    StatusMsg = "БД недоступна, повторная попытка раз в 10 сек";
                    try
                    {
                        await Task.Delay(10000, ct);
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        DBOK = false;
                        return;
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }
                
            }
            while (!DBOK);
            
        }

        

        private DelegateCommand _CloseAppCommand;
        public DelegateCommand CloseAppCommand =>
            _CloseAppCommand ?? (_CloseAppCommand = new DelegateCommand(ExecuteCloseAppCommand));

        void ExecuteCloseAppCommand()
        {
            cts.Cancel();
            Application.Current.Shutdown();
        }


        private DelegateCommand _UpdateDataCommand;
        public DelegateCommand UpdateDataCommand =>
            _UpdateDataCommand ?? (_UpdateDataCommand = new DelegateCommand(ExecuteUpdateDataCommand));

        void ExecuteUpdateDataCommand()
        {
            _EA.GetEvent<UpdateMsg>().Publish("UpdateAll");
        }
        



        public MainWindowViewModel(IManageWorkers service, IEventAggregator ea, IRegionManager regionManager)
        {
            _Service = service;
            _EA = ea;
            _RegionManager = regionManager;
            _RegionManager.RegisterViewWithRegion("PopulateRegion", typeof(PopulateView))
                .RegisterViewWithRegion("StatRegion", typeof(StatView))
                .RegisterViewWithRegion("WorkerRegion", typeof(WorkerView))
                ;
        }

        public MainWindowViewModel()

        {
            if (!App.IsDesignTime) throw new InvalidOperationException("Данный конструктор не предназначен для использования вне дизайнера VisualStudio");


        }
    }
}
