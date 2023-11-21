using AutoMapper;
using CarPool.DAL.Entities;



namespace CarPool.BL
{
    public record RideListModel(
        string StartLocation,
        string EndLocation,
        DateTime StartTime,
        DateTime EndTime) : ModelBase
    {
        public string StartLocation { get; set; } = StartLocation;
        public string EndLocation { get; set; } = EndLocation;
        public DateTime StartTime { get; set; } = StartTime;
        public DateTime EndTime { get; set; } = EndTime;
       

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<RideEntity, RideListModel>();
            }
        }
        public static RideListModel Empty => new(string.Empty, string.Empty, default, default);
    }
}
