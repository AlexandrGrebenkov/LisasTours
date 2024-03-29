﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LisasTours.Data;
using LisasTours.Models;
using LisasTours.Models.ViewModels;
using LisasTours.Services;
using Microsoft.EntityFrameworkCore;

namespace LisasTours.Application.Queries
{
    public class CompanyQueries : ICompanyQueries
    {
        private readonly ApplicationDbContext context;
        private readonly CompanyFilterService companyFilterService;

        public CompanyQueries(ApplicationDbContext context,
                              CompanyFilterService companyFilterService)
        {
            this.context = context;
            this.companyFilterService = companyFilterService;
        }

        public async Task<IEnumerable<Company>> GetCompanies(PagingVM paging = null, CompanySearchVM search = null)
        {
            var companies = context.Company
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Include(c => c.Contacts).ThenInclude(c => c.ContactType).AsQueryable();

            companies = ApplyFiltering(search, companies);
            companies = ApplyPaging(paging, companies);

            return await companies.ToListAsync();
        }

        private IQueryable<Company> ApplyFiltering(CompanySearchVM search, IQueryable<Company> companies)
        {
            if (search != null)
            {
                companies = companies.Where(companyFilterService.CreateCompanyFilterExpression(search));
            }
            return companies;
        }

        private IQueryable<Company> ApplyPaging(PagingVM paging, IQueryable<Company> companies)
        {
            if (paging != null)
            {
                companies = companies.Skip(paging.PageIndex * paging.PageSize)
                                .Take(paging.PageSize);
            }
            return companies;
        }

        public Company GetCompany(int id)
        {
            return context.Company
                .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Include(c => c.Contacts).ThenInclude(contact => contact.ContactType)
                .FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<BusinessLine> GetBusinessLines()
        {
            return context.Set<BusinessLine>();
        }

        public IEnumerable<Region> GetRegions()
        {
            return context.Set<Region>();
        }
    }
}
