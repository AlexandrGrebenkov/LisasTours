using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LisasTours.Models
{
    public class Contact
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string Mail { get; set; }

        public int ContactTypeId { get; set; }
        public ContactType Type { get; set; }

        public string FirstName { get; set; }
        public string PatronymicName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
