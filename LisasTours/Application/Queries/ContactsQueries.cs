using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LisasTours.Data;
using LisasTours.Models;
using LisasTours.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LisasTours.Application.Queries
{
    public class ContactsQueries : IContactsQueries
    {
        private readonly ApplicationDbContext context;

        public ContactsQueries(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Contact>> GetContacts(PagingVM paging = null)
        {
            var contacts = context.Contacts
                .Include(c => c.ContactType)
                .Include(c => c.Company)
                .AsQueryable();

            contacts = ApplyPaging(paging, contacts);

            return await contacts.ToListAsync();
        }

        private IQueryable<Contact> ApplyPaging(PagingVM paging, IQueryable<Contact> contacts)
        {
            if (paging != null)
            {
                contacts = contacts.Skip(paging.PageIndex * paging.PageSize)
                                .Take(paging.PageSize);
            }
            return contacts;
        }

        public Contact GetContact(int id)
        {
            return context.Contacts
                .Include(c => c.ContactType)
                .Include(c => c.Company)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<ContactType> GetContactTypes()
        {
            return context.Set<ContactType>();
        }
    }
}
