using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using LisasTours.Application.Commands;
using LisasTours.Application.Queries;
using LisasTours.Data;
using LisasTours.Models;
using LisasTours.Models.ViewModels;
using LisasTours.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LisasTours.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ExcelExporter exporter;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ICompanyQueries companyQueries;

        public CompaniesController(ApplicationDbContext context,
                                   ExcelExporter exporter,
                                   IMediator mediator,
                                   ICompanyQueries companyQueries,
                                   IMapper mapper)
        {
            this.exporter = exporter;
            this.mediator = mediator;
            this.companyQueries = companyQueries;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await companyQueries.GetCompanies(null, null);
            return View(companies);
        }

        public async Task<IActionResult> Search(CompanySearchVM searchVM)
        {
            var companies = await companyQueries.GetCompanies(null, searchVM);
            return View(nameof(Index), companies);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var company = await companyQueries.GetCompany(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        public IActionResult Create()
        {
            CreateViewDataForChanges();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCompanyVM vm)
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var company = await companyQueries.GetCompany(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<CreateCompanyVM>(company);
            CreateViewDataForChanges();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateCompanyVM vm)
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var company = await companyQueries.GetCompany(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
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
            ViewData["ContactTypes"] = companyQueries.GetContactTypes();
        }
    }
}
