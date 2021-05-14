using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        private void CreateUserMaps()
        {
            CreateMap<User, User>();
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }

        private void CreatePassionMaps()
        {
            CreateMap<Passion, Passion>();
            CreateMap<PassionCreate, Passion>();
            CreateMap<PassionUpdate, Passion>();
            CreateMap<PassionRequest, Passion>();
            CreateMap<Passion, PassionResponse>();
        }

        private void CreateHateMaps()
        {
            CreateMap<Hate, Hate>();
            CreateMap<HateCreate, Hate>();
            CreateMap<HateUpdate, Hate>();
            CreateMap<HateRequest, Hate>();
            CreateMap<Hate, HateResponse>();
        }

        private void CreateMatchMaps()
        {
            CreateMap<Match, Match>();
            CreateMap<MatchRequest, Match>();
        }

        private void CreateDateMaps()
        {
            CreateMap<Date, Date>();
            CreateMap<Date, DateResponse>();
        }

        private void CreateCloudinaryImageMaps()
        {
            CreateMap<CloudinaryImage, CloudinaryImage>();
            CreateMap<CloudinaryImage, CloudinaryImageResponse>();
        }
        
        private void CreateChatMaps()
        {
            CreateMap<ChatMessageRequest, ChatMessage>();
        }

        public MappingProfiles()
        {
            CreateUserMaps();
            CreatePassionMaps();
            CreateHateMaps();
            CreateMatchMaps();
            CreateDateMaps();
            CreateCloudinaryImageMaps();
            CreateChatMaps();
        }
    }
}