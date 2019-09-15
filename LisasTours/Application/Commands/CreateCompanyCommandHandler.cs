using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LisasTours.Data;
using LisasTours.Models;
using LisasTours.Models.Base;
using MediatR;

namespace LisasTours.Application.Commands
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

            var company = new Company()
            {
                Name = vm.Name,
                Site = vm.Site,
                Information = vm.Information,
                BusinessLines = GetNamedCollection<BusinessLine>(vm.BusinessLineNames)
                    .Select(_ => new CompanyBusinessLine() { BusinessLine = _ })
                    .ToList(),
                Affiliates = GetNamedCollection<Region>(vm.AffiliationNames)
                    .Select(_ => new Affiliate() { Region = _ })
                    .ToList(),                
                Contacts = vm.Contacts
                    .Where(_=> !string.IsNullOrWhiteSpace(_.FirstName) ||
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

            var types = GetNamedCollection<ContactType>(company.Contacts.Select(_ => _.ContactType.Name))
                    .ToList();
            foreach (var contact in company.Contacts)
            {
                contact.ContactType = types.First(_ => _.Name == contact.ContactType.Name);
            }

            context.Add(company);
            await context.SaveChangesAsync();
            return true;
        }

        private IEnumerable<T> GetNamedCollection<T>(IEnumerable<string> names)
            where T : NamedEntity, new()
        {
            var newCollection = names
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(str => new T() { Name = str })
                .ToList();
            var collectionFromDb = context.Set<T>().ToList();

            // колхоз для получения Id по стоке названия
            foreach (var item in newCollection)
            {
                var bb = collectionFromDb.FirstOrDefault(_ => _.Name == item.Name);
                if (bb != null)
                {
                    item.Id = bb.Id;
                }
            }
            return newCollection;
        }
    }
}
