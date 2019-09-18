using System.ComponentModel.DataAnnotations.Schema;
using LisasTours.Models.Base;

namespace LisasTours.Models
{
    public class Contact : Entity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string Mail { get; set; }

        public int ContactTypeId { get; set; }
        public ContactType ContactType { get; set; }

        public string FirstName { get; set; }
        public string PatronymicName { get; set; }
        public string LastName { get; set; }
    }
}
