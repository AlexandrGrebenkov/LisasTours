using LisasTours.Models.ViewModels;
using MediatR;

namespace LisasTours.Application.Commands.Contacts
{
    public class UpdateContactCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public ContactVM Contact { get; set; }

        public UpdateContactCommand(int id, ContactVM contact)
        {
            Id = id;
            Contact = contact;
        }
    }
}
