using AutoMapper;
using LisasTours.Models;
using LisasTours.Models.Identity;
using LisasTours.Models.ViewModels;

namespace LisasTours.Application.Mapping
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<ApplicationUser, UserVM>();
        }
    }
}
