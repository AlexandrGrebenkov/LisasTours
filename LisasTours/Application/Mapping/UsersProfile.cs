using AutoMapper;
using LisasTours.Models.Identity;
using LisasTours.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace LisasTours.Application.Mapping
{
    public class UsersProfile : Profile
    {
        public UsersProfile(UserManager<ApplicationUser> userManager)
        {
            CreateMap<ApplicationUser, UserVM>().AfterMap(async (src, dst) =>
            {
                dst.UserRoles = string.Join(", ", await userManager.GetRolesAsync(src));
            });
        }
    }
}
