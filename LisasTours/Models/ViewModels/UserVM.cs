using System.Collections.Generic;

namespace LisasTours.Models.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
