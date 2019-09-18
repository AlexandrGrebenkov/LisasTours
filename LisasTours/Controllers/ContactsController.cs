using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper mapper;

        public ContactsController(IContactsQueries contactsQueries,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            this.contactsQueries = contactsQueries;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = mapper.Map<ContactVM[]>(await contactsQueries.GetContacts());
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

            return View(mapper.Map<ContactVM>(contact));
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
            return View(mapper.Map<ContactVM>(contact));
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

            return View(mapper.Map<ContactVM>(contact));
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
