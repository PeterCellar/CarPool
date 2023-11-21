using System.Collections.ObjectModel;
using CarPool.BL;
using CarPool.BL.Facades;
using CookBook.App.ViewModels;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using CarPool.App.Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarPool.App.ViewModels
{
    public class DriverViewModel : ViewModelBase
    {

        private readonly IMessenger _messenger;
        private UserDetailModel selectedUser;
        private CarListModel selectedCar;
        private RideListModel selectedRide;
        private UserDetailModel selectedPassenger;

        // needed facades
        private readonly RideFacade _rideFacade;
        private readonly UserFacade _userFacade;
        private readonly CarFacade _carFacade;
        private readonly NumberOfRidesFacade _numberOfRidesFacade;

        // command for removing passenger out of car
        public ICommand MyCarsCmd { get; }
        public ICommand MyRidesCmd { get; }

        public ICommand ShowPassengersCmd { get; }

        // commands for adding new car or editing existing car
        public ICommand AddCarCmd { set; get; }
        public ICommand AddRideCmd { set; get; }

        // commands for deleting car/ride
        public ICommand DeleteCarCmd { set; get; }
        public ICommand DeleteRideCmd { set; get; }
        public ICommand DeletePassengerCmd { set; get; }

        // command for clear the form
        public ICommand ClearFormsCommand { get; }

        public ICommand UpdateCarCmd { set; get; }
        public ICommand SelectCarCmd { set; get; }

        public ICommand SelectRideCmd { get; }

        //

        //public ICommand SelectCarCmd { get;  }
        //public ICommand SelectRideCmd { get; }

        public ICommand LoadPassengersCmd { get;  }

        private ObservableCollection<CarListModel> cars = new ObservableCollection<CarListModel>(); 
        private ObservableCollection<RideListModel> rides = new ObservableCollection<RideListModel>(); 
        private ObservableCollection<UserDetailModel> passengers = new ObservableCollection<UserDetailModel>(); 
        private ObservableCollection<NumberOfRidesDetailModel> numberOfRides = new ObservableCollection<NumberOfRidesDetailModel>(); 

        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public ObservableCollection<CarListModel> Cars
        {
            get { return cars; }
            private set
            {
                cars = value;
                OnPropertyChanged();
            }
        }

        // manufacturer for creating a new car and editing already existing data
        public string Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                _manufacturer = value;
                OnPropertyChanged(nameof(Manufacturer));
            }
        }
        private string _manufacturer;
        private string _manufacturerInfo;

        public string ManufacturerInfo
        {
            get { return _manufacturerInfo; }
            set
            {
                _manufacturerInfo = value;
                OnPropertyChanged(nameof(ManufacturerInfo));
            }
        }

        // type for creating a new car and editing already existing one
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        private string _type;

        public string TypeInfo
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(TypeInfo));
            }
        }

        // first registration for creating a new car and editing already existing one
        public System.DateTime FirstReg
        {
            get { return _firstReg; }
            set
            {
                _firstReg = value;
                OnPropertyChanged(nameof(FirstReg));
            }
        }
        private System.DateTime _firstReg;
        public System.DateTime FirstRegistrationInfo
        {
            get { return _firstReg; }
            set
            {
                _firstReg = value;
                OnPropertyChanged(nameof(FirstRegistrationInfo));
            }
        }
        
        public int Seats
        {
            get { return _seats; }
            set
            {
                _seats = value;
                OnPropertyChanged(nameof(Seats));
            }
        }
        private int _seats;
        public int SeatsInfo
        {
            get { return _seats; }
            set
            {
                _seats = value;
                OnPropertyChanged(nameof(SeatsInfo));
            }
        }

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
        public string ImageUrlInfo
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrlInfo));
            }
        }

        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ObservableCollection<RideListModel> Rides
        {
            get { return rides; }
            private set
            {
                rides = value;
                OnPropertyChanged();
            }
        }
        public string StartLocation
        {
            get { return _startLocation; }
            set
            {
                _startLocation = value;
                OnPropertyChanged(nameof(StartLocation));
            }
        }
        private string _startLocation;

        public string EndLocation
        {
            get { return _endLocation; }
            set
            {
                _endLocation = value;
                OnPropertyChanged(nameof(EndLocation));
            }
        }
        private string _endLocation;



        //public string Location = _startLocation + "-" + _endLocation;

        public DateTime StartTime
        {
            get { return _start; }
            set
            {
                _start = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }
        private DateTime _start;

        public DateTime EndTime
        {
            get { return _end; }
            set
            {
                _end = value;
                OnPropertyChanged(nameof(EndTime));
            }
        }
        private DateTime _end;

        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ObservableCollection<NumberOfRidesDetailModel> NumberOfRides
        {
            get { return numberOfRides; }
            private set
            {
                numberOfRides = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserDetailModel> Passengers
        {
            get { return passengers;  }
            private set
            {
                passengers = value;
                OnPropertyChanged();
            }
        }

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

        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                 CONSTRUCTOR 
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public DriverViewModel( UserFacade userFacade, CarFacade carFacade, RideFacade rideFacade, NumberOfRidesFacade numberOfRidesFacade, IMessenger messenger)
        {
            _messenger = messenger;
            _carFacade = carFacade;
            _userFacade = userFacade;
            _rideFacade = rideFacade;
            _numberOfRidesFacade = numberOfRidesFacade;
            _messenger.Register<VisibleMessage>(SetVisibility);
            _messenger.Register<UserSelectedMessage>(SetUser);
            visible = null;

            MyCarsCmd = new RelayCommand(MyCars);
            MyRidesCmd = new RelayCommand(MyRides);
            ShowPassengersCmd = new RelayCommand(ShowPassengers);

            AddCarCmd = new RelayCommand(AddCar);
            AddRideCmd = new RelayCommand(AddRide);
            
            DeleteCarCmd = new RelayCommand(DeleteCar);
            DeleteRideCmd = new RelayCommand(DeleteRide);
            DeletePassengerCmd = new RelayCommand(DeletePassenger);

            UpdateCarCmd = new RelayCommand(UpdateCar);
            SelectCarCmd = new RelayCommand(SelectCar);

            ImageUrl = "https://d2gg9evh47fn9z.cloudfront.net/1600px_COLOURBOX5558916.jpg";

        }

        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void SelectCar()
        {
            if (SelectedCar != null)
            {
                ManufacturerInfo = SelectedCar.Manufacturer;
                TypeInfo = SelectedCar.Type;
                FirstRegistrationInfo = SelectedCar.FirstRegistration;
                SeatsInfo = SelectedCar.Seats;
                ImageUrlInfo = SelectedCar.ImageUrl;
                ImageUrl = SelectedCar.ImageUrl;
            }

        }

        private async void UpdateCar()
        {
            if (selectedUser != null && selectedCar != null)
            {
                selectedUser.CarsOwned.Remove(selectedCar);
                CarDetailModel new_car = new CarDetailModel(ManufacturerInfo, TypeInfo, FirstRegistrationInfo, selectedUser.Id, SeatsInfo);
                new_car.ImageUrl = ImageUrlInfo;

                new_car.Id = selectedCar.Id;
                var ret = await _carFacade.SaveAsync(new_car); // exception: userentity tracked elsewhere -> key id error
                var users2 = await _carFacade.GetAsync();
                var help = await _carFacade.GetAsync(selectedCar.Id);
                await _messenger.Send(new LoadDataMessage());

                CarListModel car_list = new CarListModel(new_car.Manufacturer, new_car.Type, new_car.FirstRegistration, new_car.Seats);
                car_list.ImageUrl = ImageUrlInfo;

                SelectedCar = car_list;
                selectedUser.CarsOwned.Add(selectedCar);


                LoadCars();
                await LoadData();

            }
            else
            {
                throw new InvalidOperationException("Trying to save empty model");
            }

        }

        private async void DeleteCar()
        {
            if (selectedCar is null)
            {
                return; 
            }

            if (selectedCar.Id != Guid.Empty)
            {
                await _carFacade.DeleteAsync(selectedCar!.Id);
                selectedUser.CarsOwned.Remove(selectedCar);
                cars.Remove(SelectedCar);

            }
        }

        private async void DeleteRide()
        {
            if (selectedRide is null)
            {
                return;
            }

            if (selectedRide.Id != Guid.Empty)
            {
                await _rideFacade.DeleteAsync(selectedRide!.Id);
                rides.Remove(SelectedRide);
            }
        }

        private async void DeletePassenger()
        {
            await _rideFacade.DeleteFromPassengersAsync(SelectedRide.Id, SelectedPassenger.Id);
            await LoadPassengers();
        }

        private async void MyCars()
        {
            await LoadCars();
        }
        private async void MyRides()
        {
            await LoadRides();
        }

        private async void ShowPassengers()
        {
            await LoadPassengers();
        }

        private async void AddCar()
        {
            var users = await _userFacade.GetAsync();
            var owner = users.Single(i => i.Id == selectedUser.Id);
            CarDetailModel car = new CarDetailModel(Manufacturer, Type, FirstReg, selectedUser.Id, Seats);
            car.ImageUrl = ImageUrl;
            var ret = await _carFacade.SaveAsync(car); // waiting for saving data
            selectedUser = await _userFacade.GetAsync(selectedUser.Id);
            ClearCarForms();
            await LoadCars();
        }
        
        private async Task LoadCars()
        {
            //var cars = await _carFacade.GetAsync();
            //Cars = new ObservableCollection<CarListModel>(cars);
            Cars = new ObservableCollection<CarListModel>(selectedUser.CarsOwned);
        }

        private async Task LoadRides()
        {
            var rides = selectedUser.Offering;
            Rides = new ObservableCollection<RideListModel>(rides);
        }

        private async Task LoadPassengers()
        {

            if (SelectedRide == null)
            {
                return;
            }
            
            var rideDet = await _rideFacade.GetAsync(selectedRide.Id);

            if(rideDet == null) { return; }
            Collection<UserDetailModel > users = new Collection<UserDetailModel>();
            var NumberofRidesList = rideDet.Passengers;
            foreach(var user in NumberofRidesList)
            {
                users.Add(user.User);
            }
            Passengers = new ObservableCollection<UserDetailModel>(users);


        }

        protected override async Task LoadData()
        {
            await base.LoadData();
            await LoadCars();
            await LoadRides();
            await LoadPassengers();
        }

        private async void AddRide()
        {
            //var rides = selectedUser.Offering;
            //Rides = new ObservableCollection<RideListModel>(rides);

            var users = await _userFacade.GetAsync();
            var owner = users.Single(i=>i.Id == selectedUser.Id);
            // TODO ak selectedCar == NULL , vyhodit na display, ze auto nebolo vybrane a exit
            if (SelectedCar == null)
            {
                return;
            }
            var cars = await _carFacade.GetAsync();
            var id = cars.Single(i => i.Id == selectedCar.Id);
            RideDetailModel ride = new RideDetailModel(StartTime, EndTime, StartLocation, selectedUser.Id, selectedCar.Id,  EndLocation);
            var ret = await _rideFacade.SaveAsync(ride); // fnuk


            RideListModel ride_list = new RideListModel(ride.StartLocation, ride.EndLocation, ride.StartTime, ride.EndTime);

            selectedUser.Offering.Add(ride_list);


            ClearRideForms();
            await LoadRides();
            await LoadData();
        }


        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool? visible = null;

        private void SetVisibility(VisibleMessage message)
        {
            if (message.Name == "Driver")
            {
                if (message.Visible == true)
                {
                    ClearCarForms();
                    ClearRideForms();
                    ClearCarFormInfo();
                    Visible = message.Visible;
                }
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
        public CarListModel SelectedCar
        {
            get { return selectedCar; }
            set
            {
                if (Equals (value, selectedCar)) return;
                selectedCar = value;
                OnPropertyChanged();
            }
        }

        public RideListModel SelectedRide
        {
            get { return selectedRide; }
            set
            {
                if (Equals(value, selectedRide)) return;
                selectedRide = value;
                OnPropertyChanged();
            }
        }

        public UserDetailModel SelectedPassenger
        {
            get { return selectedPassenger;  }
            set
            {
                if (Equals(value, selectedPassenger)) return;
                selectedPassenger = value;
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

        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ClearCarFormInfo()
        {
            ManufacturerInfo = "";
            TypeInfo = "";
            FirstRegistrationInfo = DateTime.Now;
            SeatsInfo = 0;
            ImageUrlInfo = "";
        }

        private void ClearCarForms()
        {
            Manufacturer = "";
            Type = "";
            FirstReg = DateTime.Now;
            Seats = 0;
            ImageUrl = "";
        }

        private void ClearRideForms()
        {
            StartLocation = "";
            EndLocation = "";
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
        }

        // Without this, the program will crash, because there is no value in image SourceUri
        public override void LoadInDesignMode()
        {
            ImageUrl = "";
        }
    }
}

/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
