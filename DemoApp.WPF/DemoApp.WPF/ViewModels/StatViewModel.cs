using DemoApp.DAL.Entityes;
using DemoApp.WPF.Core.Messages;
using DemoApp.WPF.Extensions;
using DemoApp.WPF.Services;
using DemoApp.WPF.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace DemoApp.WPF.ViewModels
{
    internal class StatViewModel :BindableBase
    {
        private readonly IManageWorkers _Service;
        private readonly XlsReport _Report;
        private IEventAggregator _EA;
        private ObservableCollection<Stat001> _StatList;
        public ObservableCollection<Stat001> StatList
        {
            get { return _StatList; }
            set { SetProperty(ref _StatList, value); }
        }


        private DelegateCommand _LoadDataCommand;
        public DelegateCommand LoadDataCommand =>
            _LoadDataCommand ?? (_LoadDataCommand = new DelegateCommand(ExecuteLoadDataCommand));

        async void ExecuteLoadDataCommand()
        {
            var stats = await _Service.GetAllStatAsync();
            StatList ??= new ObservableCollection<Stat001>();
            StatList.AddClear(stats);
        }

        private DelegateCommand _XlsCommand;

        public DelegateCommand XlsCommand =>
            _XlsCommand ?? (_XlsCommand = new DelegateCommand(ExecuteXlsCommand));

        void ExecuteXlsCommand()
        {
            _Report.XlsExportFromList(StatList, "/XLS/STAT001.xlsx", $"\\OUT\\");
        }
        public StatViewModel(IManageWorkers service, XlsReport report, IEventAggregator ea)
        {
            _Service = service;
            _Report = report;
            _EA = ea;
            _EA.GetEvent<DbConnectMsg>().Subscribe(MessageReceived);
            _EA.GetEvent<UpdateMsg>().Subscribe(UpdateAll);
        }

        private void UpdateAll(string msg)
        {
            if (msg == "UpdateAll")
            {
                LoadDataCommand.Execute();
            }
        }

        private void MessageReceived(string msg)
        {
            if (msg == "DBOK")
            {
                LoadDataCommand.Execute();
            }
        }

        public StatViewModel()
        {
                ///
        }
    }
}
