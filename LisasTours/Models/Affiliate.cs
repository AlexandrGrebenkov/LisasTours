using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LisasTours.Models
{
    /// <summary>
    /// Филиал компании (представительство)
    /// </summary>
    public class Affiliate
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
