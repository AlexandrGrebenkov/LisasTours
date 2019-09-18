using System.Linq;
using System.Threading.Tasks;
using LisasTours.Application.Queries;
using LisasTours.Data;
using LisasTours.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LisasTours.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IContactsQueries contactsQueries;

        public ContactsController(ApplicationDbContext context, IContactsQueries contactsQueries)
        {
            _context = context;
            this.contactsQueries = contactsQueries;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await contactsQueries.GetContacts();
            return View(contacts);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contact = contactsQueries.GetContact(id.Value);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contact = contactsQueries.GetContact(id.Value);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["ContactTypeId"] = new SelectList(_context.ContactTypes, "Id", "Name", contact.ContactTypeId);
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Mail,ContactTypeId,FirstName,PatronymicName,LastName,Id")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContactTypeId"] = new SelectList(_context.ContactTypes, "Id", "Name", contact.ContactTypeId);
            return View(contact);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contact = contactsQueries.GetContact(id.Value);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
