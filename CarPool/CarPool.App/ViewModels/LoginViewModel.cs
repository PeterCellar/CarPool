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
    public class LoginViewModel: ViewModelBase
    {

        private readonly UserFacade _userFacade;
        private readonly IMessenger _messenger;
        private bool? visible = true;

        public ICommand AddUserCommand { get; }
        public ICommand ClearFormsCommand { get; }
        public ICommand SelectUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        private ObservableCollection<UserListModel> users = new ObservableCollection<UserListModel>();
        private UserListModel selectedUser;


        public ObservableCollection<UserListModel> Users
        {
            get { return users; }
            private set
            {
                users = value;
                OnPropertyChanged();
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        private string _surname;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _name;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        private string _username;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }
        private string _imageUrl;

        public UserListModel SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (Equals(value, selectedUser)) return;
                selectedUser = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel(UserFacade userFacade, IMessenger messenger)
        {
            _userFacade = userFacade;
            _messenger = messenger;
            _messenger.Register<VisibleMessage>(SetVisibility);
            _messenger.Register<LoadDataMessage>(DeleteAccount);
            AddUserCommand = new RelayCommand(AddUser);
            ClearFormsCommand = new RelayCommand(Clearforms);
            SelectUserCommand = new RelayCommand(SelectUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
        }

        

        private void SetVisibility(VisibleMessage message)
        {
            Console.WriteLine(message);
            if (message.Name == "Login")
            {
                Visible = message.Visible;
            }
        }

        public bool? Visible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    OnPropertyChanged();
                }
            }
        }

        private async void SelectUser()
        {
            if (SelectedUser != null)
            {
                Console.WriteLine(selectedUser.Name);
                UserDetailModel new_user = await _userFacade.GetAsync(selectedUser.Id);
                await _messenger.Send(new UserSelectedMessage(new_user));
                Visible = null;
                await _messenger.Send(new VisibleMessage(true, "Navbar"));
                await _messenger.Send(new LoadDataMessage());
                //_messenger.Send(new VisibleMessage(true, "Test")); PUT THERE SOMETHING EXCEPT TEST
            }

        }

        protected override async Task LoadData()
        {
            await base.LoadData();
            await LoadUsers();
        }

        private async void AddUser() {
            UserDetailModel usr = new UserDetailModel(Username, Name, Surname);
            usr.ImageUrl = ImageUrl;
            var ret = await _userFacade.SaveAsync(usr);
            Clearforms();
            await LoadUsers();
        }

        private async void DeleteUser()
        {
            if (selectedUser is null)
            {
                return;
                //throw new InvalidOperationException("Null model cannot be deleted");
            }

            if (selectedUser.Id != Guid.Empty)
            {
                await _userFacade.DeleteAsync(selectedUser!.Id);
                users.Remove(SelectedUser);
            }
        }

        private async void DeleteAccount(LoadDataMessage obj)
        {
            await LoadUsers();
        }

        private void Clearforms()
        {
            Name = "";
            Surname = "";
            Username = "";
            ImageUrl = "";

        }

        private async Task LoadUsers()
        {
            var users = await _userFacade.GetAsync();
            Users = new ObservableCollection<UserListModel>(users);
        }



    }
}
