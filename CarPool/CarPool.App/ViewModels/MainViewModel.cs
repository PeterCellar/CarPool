using CarPool.App.Services;
using CarPool.BL.Facades;
using CarPool.App.Messages;
using CookBook.App.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CarPool.BL;

namespace CarPool.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
       
        private readonly IMessenger _messenger;

        public MainViewModel(UserFacade userFacade, CarFacade carFacade, NumberOfRidesFacade numberOfRidesFacade, IMessenger messenger, RideFacade rideFacade)
        {
            loginViewModel = new LoginViewModel(userFacade, messenger);
            navbarViewModel = new NavbarViewModel( messenger);
            passengerViewModel = new PassengerViewModel(userFacade, rideFacade, numberOfRidesFacade, carFacade, messenger);
            driverViewModel = new DriverViewModel(userFacade, carFacade, rideFacade, numberOfRidesFacade, messenger);
            profileViewModel = new ProfileViewModel(userFacade, messenger);
            testViewModel = new TestViewModel(messenger);
            loginViewModel.StartLoadData();

            _messenger = messenger;
        }

        public LoginViewModel loginViewModel { get; set; }
        public NavbarViewModel navbarViewModel { get; set; }
        public TestViewModel testViewModel { get; set; }
        public PassengerViewModel passengerViewModel { get; set; }
        public DriverViewModel driverViewModel { get; set; }
        public ProfileViewModel profileViewModel { get; set; }

    }
}
