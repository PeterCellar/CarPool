using CarPool.App.Messages;
using CarPool.BL.Facades;
using CookBook.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace CarPool.App.ViewModels
{
    public class NavbarViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        public bool? visible = null;
        public ICommand LogOutCommand { get; }
        public ICommand DriverCommand { get; }
        public ICommand PassengerCommand { get; }
        public ICommand UserCommand { get; }

        public NavbarViewModel( IMessenger messenger)
        {
            _messenger = messenger;
            visible = null;
            _messenger.Register<VisibleMessage>(SetVisibility);
            LogOutCommand = new RelayCommand(LogOut);
            DriverCommand = new RelayCommand(Driver);
            UserCommand = new RelayCommand(User);
            PassengerCommand = new RelayCommand(Passenger);
        }

        private void SetVisibility(VisibleMessage message)
        {
            if(message.Name == "Navbar")
            {
                if (message.Visible == true)
                    Visible = message.Visible;
                else
                    Visible = null;
            }
        }

        public bool? Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }

        private void LogOut()
        {
            SetVisibility(new VisibleMessage(false, "Navbar"));
            _messenger.Send(new VisibleMessage(false, "Driver"));
            _messenger.Send(new VisibleMessage(false, "Passenger"));
            _messenger.Send(new VisibleMessage(false, "User"));
            _messenger.Send(new VisibleMessage(true, "Login"));
        }

        private void Driver()
        {
            _messenger.Send(new VisibleMessage(true, "Driver"));
         
            _messenger.Send(new VisibleMessage(false, "User"));
            _messenger.Send(new VisibleMessage(false, "Passenger"));

        }

        private void User()
        {
            _messenger.Send(new VisibleMessage(true, "User"));

            _messenger.Send(new VisibleMessage(false, "Driver"));
            _messenger.Send(new VisibleMessage(false, "Passenger"));
        }

        private void Passenger()
        {
            _messenger.Send(new VisibleMessage(true, "Passenger"));

            _messenger.Send(new VisibleMessage(false, "Driver"));
            _messenger.Send(new VisibleMessage(false, "User"));

        }
    }
}
