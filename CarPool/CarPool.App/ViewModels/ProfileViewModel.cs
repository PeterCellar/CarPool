using CarPool.App.Messages;
using CarPool.BL;
using CarPool.BL.Facades;
using CookBook.App.ViewModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarPool.App.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly UserFacade _userFacade;
        private readonly IMessenger _messenger;
        private bool? visible = null;

        public ICommand DeleteAccountCommand { get; }
        public ICommand ConfirmChangesCommand { get; }
        //public ICommand GetImageCommand { get; }
        private UserDetailModel selectedUser;


        public ProfileViewModel(UserFacade userFacade, IMessenger messenger)
        {
            _userFacade = userFacade;
            _messenger = messenger;
            DeleteAccountCommand = new RelayCommand(DeleteUser);
            ConfirmChangesCommand = new RelayCommand(ConfirmChanges);
            _messenger.Register<UserSelectedMessage>(SetUser);
            _messenger.Register<VisibleMessage>(SetVisibility);
            _messenger.Register<UserSelectedMessage>(SetUser);
            visible = null;
            // Defulat img
            ImageUrl = "https://cdn3.iconfinder.com/data/icons/users-groups/512/group_man_woman_lead-512.png";
        }


        public void SetUser(UserSelectedMessage message)
        {
            selectedUser = message.User;
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

        private void SetVisibility(VisibleMessage message)
        {
            if (message.Name == "User")
            {
                if (message.Visible == true)
                {
                    Visible = message.Visible;

                    // Fill info about user
                    UserName = selectedUser.UserName;
                    Surname = selectedUser.Surname;
                    Name = selectedUser.Name;
                }
                else
                {
                    Visible = null;
                }
            }
        }

        public bool? Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                OnPropertyChanged();

            }
        }


        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        private string _username;

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



        // Should return to Home/Login page
        private async void DeleteUser()
        {
            if (selectedUser is not null)
            {
                if (selectedUser.Id != Guid.Empty)
                {
                    try
                    {
                        await _userFacade.DeleteAsync(selectedUser!.Id);
                        await _messenger.Send(new LoadDataMessage());
                    }
                    catch
                    {
                        // Zatial ziadna sprava 
                        NotImplementedException();
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("BAAD");
            }

            // Account do not exists
            // Home/login page should be visible
            //_messenger.Send(new VisibleMessage(true, "home"));
            await _messenger.Send(new VisibleMessage(false, "User"));
            await _messenger.Send(new VisibleMessage(true, "Login"));
        }



        public async void ConfirmChanges()
        {
            if (Name != null)
            {
                selectedUser.Name = Name;
            }
            if (Surname != null)
            {
                selectedUser.Surname = Surname;
            }
            if (UserName != null)
            {
                selectedUser.UserName = UserName;
                //Trace.WriteLine(selectedUser.UserName);
            }
            if (ImageUrl != null)
            {
                selectedUser.ImageUrl = ImageUrl;
            }

            if (selectedUser != null &&  selectedUser.UserName != null)
            {
                UserDetailModel newusr = new UserDetailModel(selectedUser.UserName, selectedUser.Name, selectedUser.Surname);
                newusr.Id = selectedUser.Id;
                var ret = await _userFacade.SaveAsync(newusr); // exception: userentity tracked elsewhere -> key id error
                var users = await _userFacade.GetAsync();
                var help = await _userFacade.GetAsync(selectedUser.Id);
                await _messenger.Send(new LoadDataMessage());
            }
            else
            {
                throw new InvalidOperationException("Trying to save empty model");
            }

        }

        // Without this, the program will crash, because there is no value in image SourceUri
        public override void LoadInDesignMode()
        {
            ImageUrl = "";
        }

        private void NotImplementedException()
        {
            throw new NotImplementedException();
        }
    }
}
