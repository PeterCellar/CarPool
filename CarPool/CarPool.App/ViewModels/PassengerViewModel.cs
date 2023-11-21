using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
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

    public static class MyEnumerable
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            var result = new ObservableCollection<T>();
            foreach (var item in source)
                result.Add(item);
            return result;
        }
    }
    public class PassengerViewModel : ViewModelBase
    {

        private List<NumberOfRidesDetailModel> userNumberOfRidesTaking = new List<NumberOfRidesDetailModel>();
        private ObservableCollection<RideDetailModel> userRidesTaking = new ObservableCollection<RideDetailModel>();
        private ObservableCollection<RideListModel> userRidesTakingList = new ObservableCollection<RideListModel>();
        private ObservableCollection<NumberOfRidesDetailModel> myRidePassengers = new ObservableCollection<NumberOfRidesDetailModel>();
        private ObservableCollection<UserDetailModel> myRidePassengersQuery = new ObservableCollection<UserDetailModel>();

        private readonly IMessenger _messenger;

        private UserDetailModel selectedUser;
        private UserDetailModel selectedUserDetail = null;
        private readonly UserFacade _userFacade;
        private readonly RideFacade _rideFacade;
        private readonly CarFacade _carFacade;
        private readonly NumberOfRidesFacade _numberOfRidesFacade;


        public string? startLocation;
        public string? endLocation;
        public string? startTime;
        public string? endTime;

        private RideDetailModel selectedMyRide = null;
        private RideListModel selectedNewRide = null;
        public ICommand SelectMyRideCommand { get; }
        public ICommand SelectNewRideCommand { get; }
        public ICommand FilterRidesCommand { get; }
        public ICommand AcceptRideCommand { get; }
        public ICommand ViewAllRidesCommand { get; }
        public ICommand UnregisterFromRideCommand { get; }


        public bool? visible = null;

        public ObservableCollection<NumberOfRidesDetailModel> MyRidePassengers
        {
            get { return myRidePassengers; }
            private set
            {
                myRidePassengers = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserDetailModel> MyRidePassengersQuery
        {
            get { return myRidePassengersQuery; }
            private set
            {
                myRidePassengersQuery = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<RideDetailModel> UserRidesTaking
        {
            get { return userRidesTaking; }
            private set
            {
                userRidesTaking = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<RideListModel> UserRidesTakingList
        {
            get { return userRidesTakingList; }
            private set
            {
                userRidesTakingList = value;
                OnPropertyChanged();
            }
        }
        public PassengerViewModel(UserFacade userFacade, RideFacade rideFacade, NumberOfRidesFacade numberOfRidesFacade, CarFacade carFacade, IMessenger messenger)
        {
            _messenger = messenger;
            _userFacade = userFacade;
            _rideFacade = rideFacade;
            _carFacade = carFacade;
            _numberOfRidesFacade = numberOfRidesFacade;
            SelectMyRideCommand = new RelayCommand(SelectMyRide);
            SelectNewRideCommand = new RelayCommand(SelectNewRide);
            AcceptRideCommand = new RelayCommand(AcceptRide);
            ViewAllRidesCommand = new RelayCommand(ViewAllRides);
            FilterRidesCommand = new RelayCommand(FilterRides);
            _messenger.Register<UserSelectedMessage>(SetUser);
            _messenger.Register<VisibleMessage>(SetVisibility);
            _messenger.Register<LoadDataMessage>(LoadRides);
            UnregisterFromRideCommand = new RelayCommand(UnregisterFromRide);
            visible = null;
        }

        private async void UnregisterFromRide()
        {
            if (selectedMyRide == null)
                return;

            _rideFacade.DeleteFromPassengersAsync(selectedMyRide.Id, selectedUser.Id);
            ClearMyRideSelection();
            await LoadData();
            await _messenger.Send(new LoadDataMessage());
            FilterRides();
            ViewAllRides();
        }

        private async void AcceptRide()
        {
            //Trace.WriteLine("\n------------------------------- RIDE ACCEPTED -------------------------------------\n");
            //Trace.WriteLine(selectedUser.Id);
            //Trace.WriteLine(selectedNewRide.Id);
            //Trace.WriteLine("\n------------------------------- RIDE ACCEPTED -------------------------------------\n");

            if (SelectedNewRide == null)
                return;
            else
            {
                try
                {
                    var ride = await _rideFacade.GetAsync(SelectedNewRide.Id);
                    NumberOfRidesDetailModel newRideTake = new NumberOfRidesDetailModel(ride, selectedUser);

                    var didUserJoinRide = await _numberOfRidesFacade.hasUserJointRide(selectedNewRide.Id, selectedUser.Id);
                    if (didUserJoinRide)
                        return;
                    else
                    {
                        await _numberOfRidesFacade.SaveAsync(newRideTake);
                        await LoadData();
                        await _messenger.Send(new LoadDataMessage());
                    }
                }
                catch (Exception ex)
                {
                    Trace.Write(ex.ToString());
                    Console.WriteLine(ex.ToString());
                }
            }
            await _messenger.Send(new LoadDataMessage());
            ViewAllRides();
        }

        public string? StartLocation
        {
            get { return startLocation; }
            set
            {
                startLocation = value;
                OnPropertyChanged(nameof(StartLocation));
            }
        }

        public string? EndLocation
        {
            get { return endLocation; }
            set
            {
                endLocation = value;
                OnPropertyChanged(nameof(EndLocation));
            }
        }

        public string? StartTime
        {
            get { return startTime; } //startTime.ToString(); }
            set
            {
                startTime = value.ToString();
                OnPropertyChanged(nameof(StartTime));
            }
        }

        public string? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value.ToString();
                OnPropertyChanged(nameof(EndTime));
            }
        }

        public string FilterFrom
        {
            get { return _filterFrom; }
            set
            {
                _filterFrom = value;
                OnPropertyChanged(nameof(FilterFrom));
            }
        }
        private string _filterFrom;
        public string FilterTo
        {
            get { return _filterTo; }
            set
            {
                _filterTo = value;
                OnPropertyChanged(nameof(FilterTo));
            }
        }
        private string _filterTo;


        public DateTime? FilterTime
        {
            get { return _filterTime; }
            set
            {
                _filterTime = value;
                OnPropertyChanged(nameof(FilterTime));

            }
        }
        private DateTime? _filterTime = DateTime.Now;

        private void SetVisibility(VisibleMessage message)
        {
            if (message.Name == "Passenger")
            {
                if (message.Visible == true)
                    Visible = message.Visible;
                else
                    Visible = null;
            }
        }

        public ObservableCollection<RideListModel?>? FilteredRides
        {
            get { return _filteredRides; }
            set
            {
                _filteredRides = value;
                OnPropertyChanged(nameof(FilteredRides));
            }
        }
        private ObservableCollection<RideListModel> _filteredRides;

        private async void FilterRides()
        {
            var FilteredRides_save = await _rideFacade.GetByPlace(FilterFrom, FilterTo);
            var FilteredRides_oc = new ObservableCollection<RideListModel>(FilteredRides_save);

            DateTime FilterTimeNonNull = FilterTime ?? DateTime.MinValue.AddDays(1);
            if (FilterTimeNonNull == DateTime.MinValue)
                FilterTimeNonNull = DateTime.MinValue.AddDays(1);


            var FilteredByTime_save = await _rideFacade.GetByTime(FilterTimeNonNull);
            var FilteredByTime_oc = new ObservableCollection<RideListModel>(FilteredByTime_save);

            var FilteredRidesCombined = new ObservableCollection<RideListModel>();

            if (FilteredRides_oc.Count() > 0 && FilteredByTime_oc.Count() > 0)
            {
                Trace.WriteLine("PRVA PODMIENKAAAAAAAAAAA");
                FilteredRidesCombined = (FilteredRides_oc.Intersect(FilteredByTime_oc)).ToObservableCollection();
            }
            else if (FilteredRides_oc.Count() > 0)
            {
                Trace.WriteLine("DRUHA PODMIENKAAAAAAAAAAA");
                FilteredRidesCombined = FilteredRides_oc;
            }
            else if (FilteredByTime_oc.Count() > 0)
            {
                Trace.WriteLine("TRETIAA PODMIENKAAAAAAAAAAA");
                FilteredRidesCombined = FilteredByTime_oc;
            }

            var FilteredRidesProcessed = new ObservableCollection<RideListModel>();

            foreach (var ridee in FilteredRidesCombined)
            {

                var rideeDetail = await _rideFacade.GetAsync(ridee.Id);
                var rideeCarDetail = await _carFacade.GetAsync(rideeDetail.UsedCar.Id);

                if (ridee.StartTime >= DateTime.Now && rideeDetail.Passengers.Count() < rideeCarDetail.Seats)
                    FilteredRidesProcessed.Add(ridee);
            }

            FilteredRides = FilteredRidesProcessed;

            Clearforms();
        }

        private void SetUser(UserSelectedMessage message)
        {
            SelectedUser = message.User;
            ViewAllRides();
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
        public RideDetailModel SelectedMyRide
        {
            get { return selectedMyRide; }
            set
            {
                if (Equals(value, selectedMyRide)) return;
                selectedMyRide = value;
                OnPropertyChanged();
            }
        }

        public RideListModel SelectedNewRide
        {
            get { return selectedNewRide; }
            set
            {
                if (Equals(value, selectedNewRide)) return;
                selectedNewRide = value;
                OnPropertyChanged();
            }
        }

        private async void SelectMyRide()
        {
            if (selectedMyRide == null)
                return;
            //SelectedMyRide = selectedMyRide;

            //var myPassengers_save = await _numberOfRidesFacade.getPassengersByRideId(selectedMyRide.Id);
            //var myPassengers_oc = new ObservableCollection<NumberOfRidesDetailModel>(myPassengers_save);

            //MyRidePassengers = myPassengers_oc;

            //var list = new List<UserDetailModel>();
            //foreach (var item in myPassengers_oc)
            //    list.Add(item.User);

            //var myRidePassengersQuery_oc = new ObservableCollection<UserDetailModel>(list);

            //MyRidePassengersQuery = myRidePassengersQuery_oc;

            StartLocation = selectedMyRide.StartLocation;
            EndLocation = selectedMyRide.EndLocation;
            StartTime = selectedMyRide.StartTime.ToString();
            EndTime = selectedMyRide.EndTime.ToString();
        }

        private async void SelectNewRide()
        {
            //SelectedMyRide = selectedMyRide;
            //StartLocation = selectedMyRide.StartLocation;
            //EndLocation = selectedMyRide.EndLocation;
            //StartTime = selectedMyRide.StartTime.ToString();
            //EndTime = selectedMyRide.EndTime.ToString();
        }

        public bool? Visible
        {
            get
            {
                Trace.WriteLine("SOMVISIBLE?????????");
                return visible;
            }
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }

        protected override async Task LoadData()
        {
            await base.LoadData();
        }

        public async void LoadRides(LoadDataMessage message)
        {

            SelectedUser = await _userFacade.GetAsync(selectedUser.Id);
            Trace.WriteLine("SELECTED USER UPDATED ID: ", SelectedUser.Id.ToString());
            //var all_rides = await _rideFacade.GetAsync();
            var NumberofRidesList = selectedUser.RidesTaking;
            Collection<RideDetailModel> rides = new Collection<RideDetailModel>();
            foreach (var ride in NumberofRidesList)
            {
                rides.Add(ride.Ride);
            }
            UserRidesTaking = new ObservableCollection<RideDetailModel>(rides);
        }

        private void Clearforms()
        {
            FilterFrom = "";
            FilterTo = "";
            FilterTime = System.DateTime.Now;
        }

        private void ClearMyRideSelection()
        {
            SelectedMyRide = null;
            StartLocation = null;
            EndLocation = null;
            StartTime = "";
            EndTime = "";
        }
        //public async void LoadSelectedRide(LoadDataMessage message)
        //{
        //    RideDetailModel new_ride = await _rideFacade.GetAsync(selectedMyRide.Id);
        //    Collection<RideDetailModel> rides = new Collection<RideDetailModel>();
        //    rides.Append(new_ride);

        //    SelectedMyRide = new ObservableCollection<RideDetailModel>(rides)[0];

        //}
        private async void ViewAllRides()
        {
            var rides = await _rideFacade.GetAsync();
            var ridesColl = new ObservableCollection<RideListModel>();

            foreach (var ride in rides)
            {
                var isUserPassenger = await _numberOfRidesFacade.hasUserJointRide(ride.Id, selectedUser.Id);
                if (!isUserPassenger)
                {
                    ridesColl.Add(ride);
                }
            }
            FilteredRides = ridesColl;
        }
    }
}
