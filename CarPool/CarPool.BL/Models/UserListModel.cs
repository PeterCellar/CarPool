using AutoMapper;
using CarPool.DAL.Entities;


namespace CarPool.BL
{
    public record UserListModel(
         string UserName,
         string Name,
         string Surname) : ModelBase
    {
        public string UserName { get; set; } = UserName;
        public string Name { get; set; } = Name;
        public string Surname { get; set; } = Surname;
        public string? ImageUrl { get; set; }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<UserEntity, UserListModel>().ReverseMap();
            }
        }

        public static UserListModel Empty => new(string.Empty, string.Empty, string.Empty);
    }
}
