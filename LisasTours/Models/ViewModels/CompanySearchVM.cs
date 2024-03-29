﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LisasTours.Models.ViewModels
{
    public class CompanySearchVM
    {
        public IEnumerable<string> CompanyNames { get; set; }
        public IEnumerable<string> RegionNames { get; set; }
        public IEnumerable<string> BusinessLines { get; set; }
    }
}
