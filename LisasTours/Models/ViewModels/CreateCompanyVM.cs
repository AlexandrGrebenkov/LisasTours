﻿using System.Collections.Generic;

namespace LisasTours.Models.ViewModels
{
    public class CreateCompanyVM
    {
        public string Name { get; set; }
        public string Site { get; set; }
        public string Information { get; set; }

        public IList<string> BusinessLineNames { get; set; }

        public IList<string> AffiliationNames { get; set; }

        public IEnumerable<ContactVM> Contacts { get; set; }
    }
}
