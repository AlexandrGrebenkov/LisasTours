using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LisasTours.Data;
using LisasTours.Models;
using MediatR;

namespace LisasTours.Application.Commands.Companies
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, bool>
    {
        private readonly ApplicationDbContext context;

        public CreateCompanyCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var vm = request.CreateCompanyVM;
            vm.Site = vm.Site?.Trim().Replace("http://", "").Replace("https://", "");
            var dbHelper = new DbHelper(context);

            var company = new Company()
            {
                Name = vm.Name,
                Site = vm.Site,
                Information = vm.Information,
                BusinessLines = dbHelper.GetNamedCollection<BusinessLine>(vm.BusinessLineNames)
                    .Select(_ => new CompanyBusinessLine() { BusinessLine = _ })
                    .ToList(),
                Affiliates = dbHelper.GetNamedCollection<Region>(vm.AffiliationNames)
                    .Select(_ => new Affiliate() { Region = _ })
                    .ToList(),
                Contacts = vm.Contacts
                    .Where(_ => !string.IsNullOrWhiteSpace(_.FirstName) ||
                               !string.IsNullOrWhiteSpace(_.LastName) ||
                               !string.IsNullOrWhiteSpace(_.Mail))
                    .Select(c => new Contact()
                    {
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        PatronymicName = c.PatronymicName,
                        Mail = c.Mail,
                        ContactType = new ContactType() { Name = c.ContactTypeName }
                    }).ToList()
            };

            var types = dbHelper.GetNamedCollection<ContactType>(company.Contacts.Select(_ => _.ContactType.Name))
                    .ToList();
            foreach (var contact in company.Contacts)
            {
                contact.ContactType = types.First(_ => _.Name == contact.ContactType.Name);
            }

            context.Add(company);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
