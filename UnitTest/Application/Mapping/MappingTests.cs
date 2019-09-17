using AutoMapper;
using LisasTours.Models;
using LisasTours.Models.ViewModels;
using NUnit.Framework;
using UnitTests.Builders;
using UnitTests.TestInfrastructure;

namespace UnitTests.Application.Mapping
{
    public class MappingTests : TestContainer
    {
        [Test]
        public void ContactMapping()
        {
            var contact = new Contact()
            {
                FirstName = "Василий",
                PatronymicName = "Иванович",
                LastName = "Кузнецов",
                Mail = "vasiliy@mail.com",
                ContactType = new ContactType() { Name = "Маркетинг" },
                Company = Create.Company("ОАО Вектор").Build()
            };

            var mapper = Get<IMapper>();
            var vm = mapper.Map<ContactVM>(contact);

            Assert.AreEqual("Кузнецов", vm.LastName);
            Assert.AreEqual("Василий", vm.FirstName);
            Assert.AreEqual("Иванович", vm.PatronymicName);
            Assert.AreEqual("vasiliy@mail.com", vm.Mail);
            Assert.AreEqual("Маркетинг", vm.ContactTypeName);
            Assert.AreEqual("ОАО Вектор", vm.CompanyName);
        }

        [Test]
        public void CompanyMapping()
        {
            var company = Create.Company("ООО Ромашки")
                .WithSite("romashki.com")
                .WithDescription("Цветы на любой вкус")
                .AddAffiliation("Свердловская область")
                .AddAffiliation("Челябинская область")
                .AddBusinessLine("Производство")
                .AddBusinessLine("Розничная торговля")
                .Build();

            var mapper = Get<IMapper>();
            var vm = mapper.Map<CreateCompanyVM>(company);

            Assert.AreEqual("ООО Ромашки", vm.Name);
            Assert.AreEqual("romashki.com", vm.Site);
            Assert.AreEqual("Цветы на любой вкус", vm.Information);
            Assert.IsTrue(vm.BusinessLineNames.Contains("Производство"));
            Assert.IsTrue(vm.BusinessLineNames.Contains("Розничная торговля"));
            Assert.IsTrue(vm.AffiliationNames.Contains("Свердловская область"));
            Assert.IsTrue(vm.AffiliationNames.Contains("Челябинская область"));
        }
    }
}
