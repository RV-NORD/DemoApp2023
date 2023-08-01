using DemoApp.DAL.Entityes;
using DemoApp.WPF.Core.Messages;
using DemoApp.WPF.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;

namespace DemoApp.WPF.ViewModels
{
    internal class WorkerViewModel : BindableBase
    {
        private IManageWorkers _Service;
        private IDialogService _DialogService;
        private IEventAggregator _EA;

        private ObservableCollection<Worker> _Workers;
        public ObservableCollection<Worker> Workers
        {
            get { return _Workers; }
            set { SetProperty(ref _Workers, value); }
        }

        private Worker _SelectedWorker;
        public Worker SelectedWorker
        {
            get { return _SelectedWorker; }
            set { SetProperty(ref _SelectedWorker, value); }
        }

        private Child _SelectedChild;
        public Child SelectedChild
        {
            get { return _SelectedChild; }
            set { SetProperty(ref _SelectedChild, value); }
        }

        private DelegateCommand _UpdateDataCommand;
        public DelegateCommand UpdateDataCommand =>
            _UpdateDataCommand ?? (_UpdateDataCommand = new DelegateCommand(ExecuteUpdateDataCommand));

        async void ExecuteUpdateDataCommand()
        {
            Workers = new ObservableCollection<Worker>(await _Service.GetAllWorkersAsync());
        }

        #region Команда редактирования работника

        private DelegateCommand<Worker> _EditWorkerCommand;
        public DelegateCommand<Worker> EditWorkerCommand =>
            _EditWorkerCommand ?? (_EditWorkerCommand = new DelegateCommand<Worker>(ExecuteEditWorkerCommand, CanExecuteEditWorkerCommand));

        bool CanExecuteEditWorkerCommand(Worker worker) => worker is not null;
        void ExecuteEditWorkerCommand(Worker worker)
        {
            _DialogService.ShowDialog("EditWorkerDialog", new DialogParameters { { "worker", worker } }, async r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    var new_worker = await _Service.EditWorkerAsync(worker);
                    if(new_worker is not null)
                    {
                        new_worker.Childs.AddRange(worker.Childs);
                        Workers.Remove(worker);
                        Workers.Add(new_worker);

                    }
                }

            });
        }

        #endregion

        #region Команда удаления работника

        private DelegateCommand<Worker> _DeleteWorkerCommand;
        public DelegateCommand<Worker> DeleteWorkerCommand =>
            _DeleteWorkerCommand ?? (_DeleteWorkerCommand = new DelegateCommand<Worker>(ExecuteDeleteWorkerCommand, CanExecuteDeleteWorkerCommand));

        bool CanExecuteDeleteWorkerCommand(Worker worker) => worker is not null;
        async void ExecuteDeleteWorkerCommand(Worker worker)
        {
            if (await _Service.DeleteWorkerAsync(worker))
                Workers.Remove(worker);
        }
        #endregion


        #region Команда добавления работника

        private DelegateCommand _AddWorkerCommand;
        public DelegateCommand AddWorkerCommand =>
            _AddWorkerCommand ?? (_AddWorkerCommand = new DelegateCommand(ExecuteAddWorkerCommand));

        void ExecuteAddWorkerCommand()
        {
            var new_worker = new Worker();
            _DialogService.ShowDialog("EditWorkerDialog", new DialogParameters { { "worker", new_worker } }, async r =>
            {

                if (r.Result == ButtonResult.OK)
                {
                    var addedWorker = await _Service.AddWorkerAsync(new_worker);
                    Workers.Add(addedWorker);
                }
                        
            });
        }

        #endregion

        #region Команда редактирования детей

        private DelegateCommand<Child> _EditChildCommand;
        public DelegateCommand<Child> EditChildCommand =>
            _EditChildCommand ?? (_EditChildCommand = new DelegateCommand<Child>(ExecuteEditChildCommand, CanExecuteEditChildCommand));

        bool CanExecuteEditChildCommand(Child child) => child is not null;
        void ExecuteEditChildCommand(Child child)
        {
            _DialogService.ShowDialog("EditChildDialog", new DialogParameters { { "child", child } }, async r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    var new_child = await _Service.EditChildAsync(child);
                    if (new_child is not null)
                    {
                        SelectedWorker.Childs.Remove(child);
                        SelectedWorker.Childs.Add(new_child);
                        ICollectionView view = CollectionViewSource.GetDefaultView(SelectedWorker.Childs);
                        view.Refresh();
                    }
                }

            });
        }

        #endregion

        #region Команда удаления детей

        private DelegateCommand<Child> _DeleteChildCommand;
        public DelegateCommand<Child> DeleteChildCommand =>
            _DeleteChildCommand ?? (_DeleteChildCommand = new DelegateCommand<Child>(ExecuteDeleteChildCommand, CanExecuteDeleteChildCommand));

        bool CanExecuteDeleteChildCommand(Child child) => child is not null;
        async void ExecuteDeleteChildCommand(Child child)
        {
            if (await _Service.DeleteChildAsync(child))
            {
                SelectedWorker.Childs.Remove(child);
                ICollectionView view = CollectionViewSource.GetDefaultView(SelectedWorker.Childs);
                view.Refresh();
            }

        }
        #endregion


        #region Команда добавления детей

        private DelegateCommand<Worker> _AddChildCommand;
        public DelegateCommand<Worker> AddChildCommand =>
            _AddChildCommand ?? (_AddChildCommand = new DelegateCommand<Worker>(ExecuteAddChildCommand, CanExecuteAddChildCommand));

        bool CanExecuteAddChildCommand(Worker worker) => worker is not null;
        void ExecuteAddChildCommand(Worker worker)
        {
            var new_child = new Child();// {Worker = worker };
            _DialogService.ShowDialog("EditChildDialog", new DialogParameters { { "child", new_child } }, async r =>
            {

                if (r.Result == ButtonResult.OK)
                {
                    var added_Child = await _Service.AddChildAsync(new_child, worker);
                    if(added_Child is not null)
                    {
                        worker.Childs.Add(added_Child);
                        ICollectionView view = CollectionViewSource.GetDefaultView(SelectedWorker.Childs);
                        view.Refresh();
                    }
                }    
            });
        }

        #endregion

        public WorkerViewModel(IManageWorkers service, IDialogService dlgservice, IEventAggregator ea)
        {
            _Service = service;
            _DialogService = dlgservice;
            _EA = ea;
            _EA.GetEvent<DbConnectMsg>().Subscribe(MessageReceived);
            _EA.GetEvent<UpdateMsg>().Subscribe(UpdateAll);
        }

        private void UpdateAll(string msg)
        {
            if (msg == "UpdateAll")
            {
                UpdateDataCommand.Execute();
            }
        }

        private void MessageReceived(string msg)
        {
            if (msg == "DBOK")
            {
                UpdateDataCommand.Execute();
            }
        }

        public WorkerViewModel()
        {
            ///
        }
    }
}
