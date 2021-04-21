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
        
        public void CreatePassionMaps()
        {
            CreateMap<Passion, Passion>();
            CreateMap<PassionCreate, Passion>();
            CreateMap<PassionRequest, Passion>();
            CreateMap<Passion, PassionResponse>();
        }

        public MappingProfiles()
        {
            CreateUserMaps();
            CreatePassionMaps();
        }
    }
}