using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LisasTours.Data;
using LisasTours.Models;
using LisasTours.Models.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LisasTours.Application.Commands.Companies
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, bool>
    {
        private readonly ApplicationDbContext context;

        public UpdateCompanyCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            PrepareData(request.CreateCompanyVM);
            var company = await LoadCompany(request.Id);
            GetEntities(request.CreateCompanyVM, out var businessLines, out var affiliates, out var contacts);
            UpdateCompanyModel(request.CreateCompanyVM, company, businessLines, affiliates, contacts);

            context.Update(company);
            await context.SaveChangesAsync();
            return true;
        }

        private void UpdateCompanyModel(CreateCompanyVM vm,
                                        Company company,
                                        IEnumerable<CompanyBusinessLine> businessLines,
                                        IEnumerable<Affiliate> affiliates,
                                        IEnumerable<Contact> contacts)
        {
            company.Name = vm.Name;
            company.Site = vm.Site;
            company.Information = vm.Information;
            company.BusinessLines = RightJoin(company.BusinessLines, businessLines, new BusinessLineComparer());
            company.Affiliates = RightJoin(company.Affiliates, affiliates, new AffiliateComparer());
            company.Contacts = RightJoin(company.Contacts, contacts, new ContactComparer());
        }

        private void GetEntities(CreateCompanyVM vm,
                                 out IEnumerable<CompanyBusinessLine> businessLines,
                                 out IEnumerable<Affiliate> affiliates,
                                 out IEnumerable<Contact> contacts)
        {
            var dbHelper = new DbHelper(context);
            businessLines = dbHelper.GetNamedCollection<BusinessLine>(vm.BusinessLineNames)
                    .Where(_ => !string.IsNullOrWhiteSpace(_.Name))
                    .Select(_ => new CompanyBusinessLine() { BusinessLine = _ });
            affiliates = dbHelper.GetNamedCollection<Region>(vm.AffiliationNames)
                    .Select(_ => new Affiliate() { Region = _ });
            contacts = vm.Contacts
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
                    }).ToList();

            var types = dbHelper.GetNamedCollection<ContactType>(contacts.Select(_ => _.ContactType.Name))
                    .ToList();
            foreach (var contact in contacts)
            {
                contact.ContactType = types.First(_ => _.Name == contact.ContactType.Name);
            }
        }

        private static void PrepareData(CreateCompanyVM vm)
        {
            vm.Site = vm.Site?.Trim().Replace("http://", "").Replace("https://", "");
        }

        private async Task<Company> LoadCompany(int id)
        {
            return await context.Company
                            .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                            .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                            .Include(c => c.Contacts).ThenInclude(contact => contact.ContactType)
                            .FirstOrDefaultAsync(m => m.Id == id);
        }

        IList<T> RightJoin<T>(IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer)
        {
            return left
                .Intersect(right, comparer)
                .Union(right, comparer)
                .ToList();
        }
    }

    class BusinessLineComparer : IEqualityComparer<CompanyBusinessLine>
    {
        public bool Equals(CompanyBusinessLine x, CompanyBusinessLine y)
        {
            return x.BusinessLine.Name == y.BusinessLine.Name;
        }

        public int GetHashCode(CompanyBusinessLine obj)
        {
            return obj.BusinessLine.Name.GetHashCode();
        }
    }

    class AffiliateComparer : IEqualityComparer<Affiliate>
    {
        public bool Equals(Affiliate x, Affiliate y)
        {
            return x.Region.Name == y.Region.Name;
        }

        public int GetHashCode(Affiliate obj)
        {
            return obj.Region.Name.GetHashCode();
        }
    }

    class ContactComparer : IEqualityComparer<Contact>
    {
        public bool Equals(Contact x, Contact y)
        {
            return x.Id == y.Id &&
                x.FirstName == y.FirstName &&
                x.PatronymicName == y.PatronymicName &&
                x.LastName == y.LastName &&
                x.Mail == y.Mail &&
                x.ContactType.Name == y.ContactType.Name;
        }

        public int GetHashCode(Contact obj)
        {
            return $"{obj.Id}{obj.FirstName}{obj.PatronymicName}{obj.LastName}{obj.Mail}{obj.ContactType.Name}".GetHashCode();
        }
    }
}
