using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using LisasTours.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace LisasTours.Models
{
    public class Company : NamedEntity
    {
        public string Site { get; set; }
        public string Information { get; set; }

        public IEnumerable<CompanyBusinessLine> BusinessLines { get; set; }

        public IEnumerable<Affiliate> Affiliates { get; set; }

        [BindProperty]
        public List<Contact> Contacts { get; set; }

        [NotMapped]
        public string BusinessLinesString => BusinessLines != null
            ? string.Join(", ", BusinessLines.Select(_ => _.BusinessLine.Name))
            : string.Empty;

        [NotMapped]
        public string AffiliatesString => Affiliates != null
            ? string.Join(", ", Affiliates.Select(_ => _.Region.Name))
            : string.Empty;
    }
}
