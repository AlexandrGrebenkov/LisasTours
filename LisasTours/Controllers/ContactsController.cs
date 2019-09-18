using System.Threading.Tasks;
using FluentValidation;
using LisasTours.Application.Commands.Contacts;
using LisasTours.Application.Queries;
using LisasTours.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LisasTours.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactsQueries contactsQueries;
        private readonly IMediator mediator;

        public ContactsController(IContactsQueries contactsQueries, IMediator mediator)
        {
            this.contactsQueries = contactsQueries;
            this.mediator = mediator;
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
            ViewData["ContactTypes"] = contactsQueries.GetContactTypes();
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactVM contact)
        {
            try
            {
                var result = await mediator.Send(new UpdateContactCommand(id, contact));
                if (!result)
                {
                    return NotFound();
                }
            }
            catch (ValidationException ex)
            {
                ViewData["ContactTypes"] = contactsQueries.GetContactTypes();
                return View(contact);
            }

            return RedirectToAction(nameof(Index));
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
            var result = await mediator.Send(new DeleteContactCommand(id));
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
