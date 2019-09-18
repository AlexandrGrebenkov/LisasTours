using MediatR;

namespace LisasTours.Application.Commands.Contacts
{
    public class DeleteContactCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteContactCommand(int id)
        {
            Id = id;
        }
    }
}
