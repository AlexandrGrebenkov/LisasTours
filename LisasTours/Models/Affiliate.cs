using LisasTours.Models.Base;

namespace LisasTours.Models
{
    /// <summary>
    /// Филиал компании (представительство)
    /// </summary>
    public class Affiliate : Entity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
