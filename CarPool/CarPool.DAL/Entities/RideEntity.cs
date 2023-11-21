namespace CarPool.DAL.Entities
{
    public record RideEntity(Guid Id,
        DateTime StartTime,
        DateTime EndTime,
        string StartLocation,
        string EndLocation,
        Guid UsedCarId,
        Guid DriverId
        ) : IEntity
    {


       public UserEntity Driver { get; init; }
       public CarEntity UsedCar { get; init; }
       public ICollection<NumberOfRidesEntity> Passengers { get; init; } = new List<NumberOfRidesEntity>();
        //public NumberOfRidesEntity NumberOfRides { get; init; }

        //Automapper requires parameter less constructor for collection synchronization for now
#nullable disable
        public RideEntity() : this(default, default, default, default, default, default, default) { }
#nullable enable
    }
}