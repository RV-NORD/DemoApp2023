using DemoApp.DAL.Entityes;
using DemoApp.WPF.ViewModels.Base;
using Prism.Services.Dialogs;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using System;
using System.ComponentModel;
using System.Linq;

namespace DemoApp.WPF.ViewModels
{
    internal class WorkerEditDialogViewModel : EditDialogViewModel
    {

        private Worker _Worker;
        public string SurName { get => GetValue(_Worker.SurName); set => SetValue(value); }
        public string FirstName { get => GetValue(_Worker.FirstName); set => SetValue(value); }
        public string LastName { get => GetValue(_Worker.LastName); set => SetValue(value); }
        public DateOnly BirthDay { get => GetValue(_Worker.BirthDay); set => SetValue(value); }
        public bool Pol { get => GetValue(_Worker.Pol); set => SetValue(value); }


        public override void Commit()
        {
            var type = _Worker.GetType();

            foreach (var (property_name, value) in _Values)
            {
                var property = type.GetProperty(property_name);
                if (property is null || !property.CanWrite) continue;

                property.SetValue(_Worker, value);
            }

            _Values.Clear();
        }

        public override void Reject()
        {
            var properties = _Values.Keys.ToArray();
            _Values.Clear();

            foreach (var property in properties)
                OnPropertyChanged(new PropertyChangedEventArgs(property));
        }


        public override void OnDialogOpened(IDialogParameters parameters)
        {
            _Worker = parameters.GetValue<Worker>("worker");
            Validator = GetValidator();
        }
        public WorkerEditDialogViewModel()
        {
            
            if (App.IsDesignTime)
                _Worker = new Worker { SurName = "Фамилия", FirstName = "Имя", LastName = "Отчество", BirthDay = new DateOnly(1990, 10, 10), Pol = true };
        }
        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<WorkerEditDialogViewModel>();

            builder.RuleFor(vm => vm.SurName)
                .NotEmpty()
                .MaxLength(16)
                .MinLength(1)
                .NotEqual("НеФамилия");
            builder.RuleFor(vm => vm.FirstName)
                .MaxLength(16)
                .MinLength(1)
                .NotEmpty()
                ;
            builder.RuleFor(vm => vm.BirthDay)
                .Between(new DateOnly(1900,1,1), new DateOnly(2010,12,12));

            return builder.Build(this);
        }
    }
}
