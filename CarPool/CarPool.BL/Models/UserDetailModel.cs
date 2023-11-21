using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL
{
    public record UserDetailModel(
        string UserName,
        string Name,
        string Surname) : ModelBase
    {
        public string UserName { get; set; } = UserName;
        public string Name { get; set; } = Name;
        public string Surname { get; set; } = Surname;
        public string? ImageUrl { get; set; }
        public List<RideListModel> Offering { get; init; } = new();
        public List<NumberOfRidesDetailModel> RidesTaking { get; init; } = new();
        public List<CarListModel> CarsOwned { get; init; } = new();

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<UserEntity, UserDetailModel>().ReverseMap();
            }
        }

        public static UserDetailModel Empty => new(string.Empty, string.Empty, string.Empty);
        // public static RideDetailModel Empty => new(default, default, string.Empty, string.Empty);
    }
}

