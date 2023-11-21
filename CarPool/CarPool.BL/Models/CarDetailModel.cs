using AutoMapper;
using CarPool.DAL.Entities;


namespace CarPool.BL
{
    public record CarDetailModel(
           string Manufacturer,
           string Type,
           DateTime FirstRegistration,
           Guid OwnerId,
           int Seats
           ) : ModelBase
    {
        public string Manufacturer { get; set; } = Manufacturer;
        public string Type { get; set; } = Type;
        public DateTime FirstRegistration { get; set; } = FirstRegistration;
        public int Seats { get; set; } = Seats;
        public string? ImageUrl { get; set; }
        public List<RideDetailModel> RidesUsedFor { get; init; } = new();
        public UserListModel Owner { get; set; }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<CarEntity, CarDetailModel>().ReverseMap();
            }
        }

        public static CarDetailModel Empty => new(string.Empty, string.Empty, default, Guid.Empty, default);
    }
}
