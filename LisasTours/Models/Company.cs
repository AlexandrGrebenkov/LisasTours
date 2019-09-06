using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LisasTours.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Site { get; set; }
        public string Information { get; set; }

        public int BusinessLineId { get; set; }
        public BusinessLine BusinessLine { get; set; }

        public IEnumerable<Affiliate> Affiliates { get; set; }

        [BindProperty]
        public List<Contact> Contacts { get; set; }
    }
}
