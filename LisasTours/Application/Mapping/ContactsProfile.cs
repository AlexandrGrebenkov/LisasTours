using System.Collections.Generic;
using AutoMapper;
using LisasTours.Models;
using LisasTours.Models.ViewModels;

namespace LisasTours.Application.Mapping
{
    public class ContactsProfile : Profile
    {
        public ContactsProfile()
        {
            CreateMap<Contact, ContactVM>().
                AfterMap((src, dest) =>
                {
                    dest.ContactTypeName = src.ContactType.Name;
                });
        }
    }
}
