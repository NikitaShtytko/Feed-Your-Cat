using AutoMapper;
using FeedYourCat.Entities;
using FeedYourCat.Models.Users;
using FeedYourCat.Models.Feeders;

namespace FeedYourCat.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<Feeder, FeederModel>();
            CreateMap<NewFeederModel, Feeder>();
            CreateMap<FeederModel, Feeder>();
            CreateMap<User, UserListModel>();
        }
    }
}