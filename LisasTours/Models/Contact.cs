﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LisasTours.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Mail { get; set; }

        public int ContactTypeId { get; set; }
        public ContactType Type { get; set; }

        public string FirstName { get; set; }
        public string PatronymicName { get; set; }
        public string LastName { get; set; }
    }
}
