using CarPool.App.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace CookBook.App.ViewModels
{
    public abstract class ViewModelBase :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected ViewModelBase()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                // ReSharper disable once VirtualMemberCallInConstructor
                LoadInDesignMode();
            }
        }
        private bool isLoaded = false;


        public bool IsLoaded
        {
            get { return isLoaded; }
            private set
            {
                if (value == isLoaded) return;
                isLoaded = value;
                OnPropertyChanged();
            }
        }

        public object NavigationParameter { get; set; }

        internal async void StartLoadData()
        {
            await LoadData();
            IsLoaded = true;
        }

        protected virtual Task LoadData()
        {
            return Task.FromResult(true);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public virtual void LoadInDesignMode() { }

    }
}