using System.Linq;
using AutoMapper;
using LisasTours.Models;
using LisasTours.Models.ViewModels;

namespace LisasTours.Application.Mapping
{
    public class CompaniesProfile : Profile
    {
        public CompaniesProfile()
        {
            CreateMap<Company, CreateCompanyVM>()
                .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Contacts))
                .AfterMap((src, dest) =>
                {
                    dest.BusinessLineNames = src.BusinessLines.Select(_ => _.BusinessLine.Name).ToList();
                    dest.AffiliationNames = src.Affiliates.Select(_ => _.Region.Name).ToList();
                });
        }
    }
}
