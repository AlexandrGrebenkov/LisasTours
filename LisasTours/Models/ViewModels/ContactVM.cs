namespace LisasTours.Models.ViewModels
{
    public class ContactVM
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }

        public string Mail { get; set; }

        public string ContactTypeName { get; set; }

        public string FirstName { get; set; }
        public string PatronymicName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{LastName} {FirstName} {PatronymicName}".TrimEnd();
    }
}
