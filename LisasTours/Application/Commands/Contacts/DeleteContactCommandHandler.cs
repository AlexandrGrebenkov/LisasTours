using System.Threading;
using System.Threading.Tasks;
using LisasTours.Data;
using MediatR;

namespace LisasTours.Application.Commands.Contacts
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand,bool>
    {
        private readonly ApplicationDbContext context;

        public DeleteContactCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await context.Contacts.FindAsync(request.Id);
            if (contact == null)
            {
                return false;
            }
            context.Contacts.Remove(contact);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
