using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarPool.BL;
using CarPool.BL.Facades;
using CarPool.App.Extensions;
using CookBook.App.ViewModels;
using System.Windows.Input;
using CarPool.App.Services;
using CarPool.App.Commands;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using CarPool.App.Messages;

namespace CarPool.App.ViewModels
{
    public class TestViewModel: ViewModelBase
    {

        private readonly IMessenger _messenger;

        private UserDetailModel selectedUser;


        public bool? visible = null;

        public TestViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            _messenger.Register<VisibleMessage>(SetVisibility);
            visible = null;
        }

        private void SetVisibility(VisibleMessage message)
        {
            if (message.Name == "Test")
            {
                if (message.Visible == true)
                    Visible = message.Visible;
                else
                    Visible = null;
            }
        }

        private void SetUser(UserSelectedMessage message)
        {
            SelectedUser = message.User;
        }

        public UserDetailModel SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (Equals(value, selectedUser)) return;
                selectedUser = value;
                OnPropertyChanged();
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
    }
}
