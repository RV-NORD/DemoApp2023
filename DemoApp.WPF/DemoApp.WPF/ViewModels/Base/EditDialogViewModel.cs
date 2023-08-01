using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using ReactiveValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DemoApp.WPF.ViewModels.Base
{
    internal abstract class EditDialogViewModel : BindableBase, IDialogAware, ReactiveValidation.IValidatableObject, INotifyDataErrorInfo
    {
        private string _title = "Редактирование";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected readonly Dictionary<string, object> _Values = new();

        protected virtual bool SetValue(object value, [CallerMemberName] string Property = null)
        {
            if (_Values.TryGetValue(Property!, out var old_value) && Equals(value, old_value))
                return false;
            _Values[Property] = value;
            CommandActive = true;
            OnPropertyChanged(new PropertyChangedEventArgs(Property));
            return true;
        }

        protected virtual T GetValue<T>(T Default, [CallerMemberName] string Property = null)
        {
            if (_Values.TryGetValue(Property!, out var value))
                return (T)value;
            return Default;
        }

        private bool _CommandActive = false;
        public bool CommandActive
        {
            get { return _CommandActive; }
            set
            {
                SetProperty(ref _CommandActive, value);
            }
        }

        private DelegateCommand _CommitCommand;
        public DelegateCommand CommitCommand =>
            _CommitCommand ?? (_CommitCommand = new DelegateCommand(ExecuteCommitCommand, CanExecuteCommitCommand)
            .ObservesProperty(() => CommandActive)
            .ObservesProperty(() => Validator.IsValid));

        private bool CanExecuteCommitCommand()
        {
            return CommandActive & Validator.IsValid;
        }

        void ExecuteCommitCommand()
        {
            Commit();
            ButtonResult result = ButtonResult.OK;
            RaiseRequestClose(new DialogResult(result));
        }

        private DelegateCommand _RejectCommand;
        public DelegateCommand RejectCommand =>
            _RejectCommand ?? (_RejectCommand = new DelegateCommand(ExecuteRejectCommand).ObservesCanExecute(() => CommandActive));


        void ExecuteRejectCommand()
        {
            Reject();
            CommandActive = false;
        }

        private DelegateCommand _CancelCommand;
        public DelegateCommand CancelCommand =>
            _CancelCommand ?? (_CancelCommand = new DelegateCommand(ExecuteCancelCommand));



        void ExecuteCancelCommand()
        {
            Reject();
            ButtonResult result = ButtonResult.Cancel;
            RaiseRequestClose(new DialogResult(result));
        }

        public abstract void Commit();
        public abstract void Reject();


        public void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
        public event Action<IDialogResult> RequestClose;


        public bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed() { }


        public virtual void OnDialogOpened(IDialogParameters parameters) { }


        private IObjectValidator? _objectValidator;

        /// <inheritdoc />
        public IObjectValidator? Validator
        {
            get => _objectValidator;
            set
            {
                _objectValidator?.Dispose();
                _objectValidator = value;
                _objectValidator?.Revalidate();
            }
        }

        /// <inheritdoc />
        public virtual void OnPropertyMessagesChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }


        #region INotifyDataErrorInfo

        /// <inheritdoc />
        bool INotifyDataErrorInfo.HasErrors => Validator?.IsValid == false || Validator?.HasWarnings == true;

        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <inheritdoc />
        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            if (Validator == null)
                return Array.Empty<ValidationMessage>();

            return string.IsNullOrEmpty(propertyName)
                ? Validator.ValidationMessages
                : Validator.GetMessages(propertyName!);
        }
        #endregion
    }
}
