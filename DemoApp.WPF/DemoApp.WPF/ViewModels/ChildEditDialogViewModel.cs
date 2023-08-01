using DemoApp.DAL.Entityes;
using DemoApp.WPF.ViewModels.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.WPF.ViewModels
{
    internal class ChildEditDialogViewModel : EditDialogViewModel
    {

        private Child _Child;
        public string FullName { get => GetValue(_Child.FullName); set => SetValue(value); }
        public DateOnly BirthDay { get => GetValue(_Child.BirthDay); set => SetValue(value); }


        public override void Commit()
        {
            var type = _Child.GetType();

            foreach (var (property_name, value) in _Values)
            {
                var property = type.GetProperty(property_name);
                if (property is null || !property.CanWrite) continue;

                property.SetValue(_Child, value);
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
            _Child = parameters.GetValue<Child>("child");
            Validator = GetValidator();
        }
        public ChildEditDialogViewModel()
        {

            if (App.IsDesignTime)
                _Child = new Child { FullName = "ФИО", BirthDay = new DateOnly(1990, 10, 10) };
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<ChildEditDialogViewModel>();

            builder.RuleFor(vm => vm.FullName)
                .NotEmpty()
                .MaxLength(16)
                .MinLength(3)
                .NotEqual("НеФИО");
            builder.RuleFor(vm => vm.BirthDay)
                .Between(new DateOnly(1950, 1, 1), DateOnly.FromDateTime(DateTime.Now));

            return builder.Build(this);
        }
    }
}
