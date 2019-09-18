using System.Collections.Generic;
using System.Threading.Tasks;
using LisasTours.Models;
using LisasTours.Models.ViewModels;

namespace LisasTours.Application.Queries
{
    public interface IContactsQueries
    {
        Contact GetContact(int id);
        Task<IEnumerable<Contact>> GetContacts(PagingVM paging = null);

        IEnumerable<ContactType> GetContactTypes();
    }
}
