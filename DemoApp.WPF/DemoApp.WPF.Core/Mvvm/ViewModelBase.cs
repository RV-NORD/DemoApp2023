using Prism.Mvvm;
using Prism.Navigation;

namespace DemoApp.WPF.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase()
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
