using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LisasTours.Data;
using LisasTours.Models;
using MediatR;

namespace LisasTours.Application.Commands.Contacts
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, bool>
    {
        private readonly ApplicationDbContext context;

        public UpdateContactCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<bool> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contact = context.Contacts.FirstOrDefault(_ => _.Id == request.Id);
            if (contact == null)
            {
                return Task.FromResult(false);
            }

            contact.FirstName = request.Contact.FirstName;
            contact.LastName = request.Contact.LastName;
            contact.PatronymicName = request.Contact.PatronymicName;
            contact.Mail = request.Contact.Mail;
            contact.ContactType = context.ContactTypes
                .FirstOrDefault(_ => _.Name == request.Contact.ContactTypeName)
                ?? new ContactType() { Name = request.Contact.ContactTypeName };

            return Task.FromResult(true);
        }
    }
}
