using System.Collections.Generic;

namespace LisasTours.Models.ViewModels
{
    public class CreateCompanyVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Site { get; set; }
        public string Information { get; set; }

        public int BusinessLineId { get; set; }
        public string BusinessLineName { get; set; }

        public int RegionId { get; set; }
        public string RegionName { get; set; }

        public IEnumerable<Contact> Contacts { get; set; }
    }
}
