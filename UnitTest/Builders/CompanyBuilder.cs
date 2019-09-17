using System.Collections.Generic;
using LisasTours.Models;

namespace UnitTests.Builders
{
    class CompanyBuilder
    {
        Company company;

        public CompanyBuilder CreateCompany(string name)
        {
            company = new Company() { Name = name };
            return this;
        }

        public CompanyBuilder WithSite(string site)
        {
            company.Site = site;
            return this;
        }

        public CompanyBuilder WithDescription(string description)
        {
            company.Information = description;
            return this;
        }

        public CompanyBuilder AddAffiliation(string regionName)
        {
            if (company.Affiliates == null)
            {
                company.Affiliates = new List<Affiliate>();
            }

            company.Affiliates.Add(Create.Entity<Affiliate>()
                .WithRegion(Create.NamedEntity<Region>(regionName)));
            return this;
        }

        public CompanyBuilder AddAffiliation(Region region)
        {
            if (company.Affiliates == null)
            {
                company.Affiliates = new List<Affiliate>();
            }

            company.Affiliates.Add(Create.Entity<Affiliate>()
                .WithRegion(region));
            return this;
        }

        public CompanyBuilder AddBusinessLine(string name)
        {
            if (company.BusinessLines == null)
            {
                company.BusinessLines = new List<CompanyBusinessLine>();
            }

            company.BusinessLines.Add(Create.Entity<CompanyBusinessLine>()
                .WithBusinessLine(Create.NamedEntity<BusinessLine>(name)));
            return this;
        }

        public CompanyBuilder AddBusinessLine(BusinessLine businessLine)
        {
            if (company.BusinessLines == null)
            {
                company.BusinessLines = new List<CompanyBusinessLine>();
            }

            company.BusinessLines.Add(Create.Entity<CompanyBusinessLine>()
                .WithBusinessLine(businessLine));
            return this;
        }

        public CompanyBuilder WithContacts(IList<Contact> contacts)
        {
            company.Contacts = contacts;
            return this;
        }


        public Company Build()
        {
            return company;
        }
    }
}
