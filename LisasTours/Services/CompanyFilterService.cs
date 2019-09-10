using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LisasTours.Helpers;
using LisasTours.Models;
using LisasTours.Models.ViewModels;

namespace LisasTours.Services
{
    public class CompanyFilterService
    {

        public Expression<Func<Company, bool>> CreateCompanyFilterExpression(CompanySearchVM searchVM)
        {
            var filters = new List<Expression<Func<Company, bool>>>()
            {
                CreateRegionFilter(searchVM.RegionNames),
            };

            return filters.And();
        }

        private static Expression<Func<Company, bool>> CreateRegionFilter(IEnumerable<string> regionNames)
        {
            if (regionNames == null)
            {
                return null;
            }

            return c => c.Affiliates.Any(_ => regionNames.Contains(_.Region.Name));
        }
    }
}
