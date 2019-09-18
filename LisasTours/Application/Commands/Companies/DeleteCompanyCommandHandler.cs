using System.Threading;
using System.Threading.Tasks;
using LisasTours.Data;
using MediatR;

namespace LisasTours.Application.Commands.Companies
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
    {
        private readonly ApplicationDbContext context;

        public DeleteCompanyCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await context.Company.FindAsync(request.Id);
            context.Company.Remove(company);
            await context.SaveChangesAsync();
            return new Unit();
        }
    }
}
