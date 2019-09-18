using System.Collections.Generic;
using LisasTours.Models.Base;

namespace LisasTours.Models
{
    public class Company : NamedEntity
    {
        public string Site { get; set; }
        public string Information { get; set; }

        public IList<CompanyBusinessLine> BusinessLines { get; set; }

        public IList<Affiliate> Affiliates { get; set; }

        public IList<Contact> Contacts { get; set; }
    }
}
