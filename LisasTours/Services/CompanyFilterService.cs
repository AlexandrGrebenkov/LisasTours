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
                CreateCompanyNamesFilter(searchVM.CompanyNames),
                CreateRegionFilter(searchVM.RegionNames),
                CreateBusinessLineFilter(searchVM.BusinessLines),
            };

            return filters.And();
        }

        private static Expression<Func<Company, bool>> CreateCompanyNamesFilter(IEnumerable<string> names)
        {
            var list = names?.ToList();
            list?.RemoveAll(_ => string.IsNullOrWhiteSpace(_));
            if (names == null || list.Count == 0)
            {
                return c => true;
            }

            return c => list.Contains(c.Name);
        }

        private static Expression<Func<Company, bool>> CreateRegionFilter(IEnumerable<string> names)
        {
            var list = names?.ToList();
            list?.RemoveAll(_ => string.IsNullOrWhiteSpace(_));
            if (names == null || list.Count == 0)
            {
                return c => true;
            }

            return c => c.Affiliates.Any(_ => list.Contains(_.Region.Name));
        }

        private static Expression<Func<Company, bool>> CreateBusinessLineFilter(IEnumerable<string> names)
        {
            var list = names?.ToList();
            list?.RemoveAll(_ => string.IsNullOrWhiteSpace(_));
            if (names == null || list.Count == 0)
            {
                return c => true;
            }

            return c => c.BusinessLines.Any(_ => list.Contains(_.BusinessLine.Name));
        }
    }
}
