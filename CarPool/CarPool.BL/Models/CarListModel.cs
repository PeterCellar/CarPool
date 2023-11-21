using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL
{
    public record CarListModel(
            string Manufacturer,
            string Type,
            // Only Date will be used
            DateTime FirstRegistration,
            int Seats) : ModelBase
    {
        public string Manufacturer { get; set; } = Manufacturer;
        public string Type { get; set; } = Type;
        public string? ImageUrl { get; set; }
        public DateTime FirstRegistration { get; set; } = FirstRegistration;
        public int Seats { get; set; } = Seats;
        public UserListModel Owner { get; set; }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<CarEntity, CarListModel>().ReverseMap();
            }
        }
    }
}
