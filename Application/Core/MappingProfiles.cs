using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public void CreateUserMaps()
        {
            CreateMap<User, User>();
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }

        public MappingProfiles()
        {
            CreateUserMaps();
        }
    }
}