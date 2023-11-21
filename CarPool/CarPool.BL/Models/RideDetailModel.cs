using AutoMapper;
using CarPool.DAL.Entities;



namespace CarPool.BL
{
    public record RideDetailModel(
        DateTime StartTime,
        DateTime EndTime,
        string StartLocation,
        Guid DriverId,
        Guid UsedCarId,
        string EndLocation) : ModelBase
    {
        public DateTime StartTime { get; set; } = StartTime;
        public DateTime EndTime { get; set; } = EndTime;
        public string StartLocation { get; set; } = StartLocation;
        public string EndLocation { get; set; } = EndLocation;
        public UserListModel Driver { get; set; }
        public CarListModel UsedCar { get; set; }
        public List<NumberOfRidesDetailModel> Passengers { get; init; } = new();
        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<RideEntity, RideDetailModel>().ReverseMap();
            }
        }

        public static RideDetailModel Empty => new(default, default, string.Empty, Guid.Empty, Guid.Empty, string.Empty);
    }
}
