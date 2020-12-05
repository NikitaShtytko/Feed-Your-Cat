using AutoMapper;
using FeedYourCat.Entities;
using FeedYourCat.Models.Users;

namespace FeedYourCat.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}