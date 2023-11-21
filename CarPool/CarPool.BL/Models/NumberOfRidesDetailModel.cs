using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL
{
    public record NumberOfRidesDetailModel(
        RideDetailModel Ride,
        UserDetailModel User
        ) : ModelBase
    {
        public RideDetailModel Ride { get; set; } = Ride;
        public UserDetailModel User { get; set; } = User;

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<NumberOfRidesEntity, NumberOfRidesDetailModel>()
                    .ReverseMap()
                    .ForMember(entity => entity.Ride, expression => expression.Ignore())
                    .ForMember(entity => entity.User, expression => expression.Ignore());
            }
        }
    }
}
