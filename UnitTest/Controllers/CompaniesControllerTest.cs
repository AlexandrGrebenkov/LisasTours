using System.Collections.Generic;
using System.Threading.Tasks;
using LisasTours.Application.Queries;
using LisasTours.Controllers;
using LisasTours.Models;
using LisasTours.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using UnitTests.Builders;
using UnitTests.TestInfrastructure;

namespace UnitTests.Controllers
{
    public class CompaniesControllerTest : BaseControllerTest<CompaniesController>
    {
        [Test]
        public async Task CreateSuccess()
        {
            var mediatrMock = GetMock<IMediator>();
            mediatrMock.Setup(m => m
                .Send(It.IsAny<IRequest<bool>>(), default))
                .Returns(Task.FromResult(true));

            var controller = GetController();
            var actionResult = await controller.Create(new CompanyVM()) as RedirectToActionResult;

            Assert.AreEqual(nameof(CompaniesController.Index), actionResult.ActionName);
        }

        [Test]
        public async Task Index()
        {
            var lukoil = Create.Company("Лукоил")
                .Build();
            var localCafe = Create.Company("Кафе у Михалыча")
                .Build();                
                
            IEnumerable<Company> companies = new[] { lukoil, localCafe };
            var queriesMock = GetMock<ICompanyQueries>();
            queriesMock.Setup(m => m
                .GetCompanies(null, null))
                .Returns(Task.FromResult(companies));

            var controller = GetController();
            var x = await queriesMock.Object.GetCompanies(null, null);
            var view = await controller.Index() as ViewResult;

            var companiesInView = view.Model as IEnumerable<Company>;

            CollectionAssert.Contains(companiesInView, lukoil);
            CollectionAssert.Contains(companiesInView, localCafe);
        }
    }
}
