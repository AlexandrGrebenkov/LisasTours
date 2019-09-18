using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using LisasTours.Application.Commands.Companies;
using LisasTours.Application.Queries;
using LisasTours.Models.ViewModels;
using LisasTours.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LisasTours.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly IExporter exporter;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ICompanyQueries companyQueries;
        private readonly IContactsQueries contactsQueries;

        public CompaniesController(IExporter exporter,
                                   IMediator mediator,
                                   ICompanyQueries companyQueries,
                                   IContactsQueries contactsQueries,
                                   IMapper mapper)
        {
            this.exporter = exporter;
            this.mediator = mediator;
            this.companyQueries = companyQueries;
            this.contactsQueries = contactsQueries;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await companyQueries.GetCompanies(null, null);
            return View(mapper.Map<CompanyVM[]>(companies));
        }

        public async Task<IActionResult> Search(CompanySearchVM searchVM)
        {
            var companies = await companyQueries.GetCompanies(null, searchVM);
            return View(nameof(Index), mapper.Map<CompanyVM[]>(companies));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var company = companyQueries.GetCompany(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(mapper.Map<CompanyVM>(company));
        }

        public IActionResult Create()
        {
            CreateViewDataForChanges();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyVM vm)
        {
            try
            {
                var result = await mediator.Send(new CreateCompanyCommand(vm));
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                // TODO: Добавить на View отображение результата валидации (пока не знаю как)
                CreateViewDataForChanges();
                return View(vm);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var company = companyQueries.GetCompany(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<CompanyVM>(company);
            CreateViewDataForChanges();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyVM vm)
        {
            try
            {
                var result = await mediator.Send(new UpdateCompanyCommand(id, vm));
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                // TODO: Добавить на View отображение результата валидации (пока не знаю как)
                CreateViewDataForChanges();
                return View(vm);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var company = companyQueries.GetCompany(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(mapper.Map<CompanyVM>(company));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await mediator.Send(new DeleteCompanyCommand(id));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Export()
        {
            var companies = await companyQueries.GetCompanies(null, null);
            var report = await exporter.GenerateCompaniesReport(companies);
            return File(report, "application/vnd.ms-excel", "Компании.xslx");
        }

        private void CreateViewDataForChanges()
        {
            ViewData["BusinessLine"] = companyQueries.GetBusinessLines();
            ViewData["Regions"] = companyQueries.GetRegions();
            ViewData["ContactTypes"] = contactsQueries.GetContactTypes();
        }
    }
}
