﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LisasTours.Models;
using LisasTours.Models.ViewModels;

namespace LisasTours.Application.Queries
{
    public interface ICompanyQueries
    {
        Task<Company> GetCompany(int id);
        Task<IEnumerable<Company>> GetCompanies(PagingVM paging = null, CompanySearchVM search = null);
    }
}
