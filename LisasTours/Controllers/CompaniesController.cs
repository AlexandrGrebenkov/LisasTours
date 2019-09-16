using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LisasTours.Application.Commands;
using LisasTours.Application.Queries;
using LisasTours.Data;
using LisasTours.Models;
using LisasTours.Models.ViewModels;
using LisasTours.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LisasTours.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ExcelExporter Exporter;
        private readonly CompanyFilterService CompanyFilterService;
        private readonly IMediator mediator;
        private readonly ICompanyQueries companyQueries;

        public CompaniesController(ApplicationDbContext context,
                                   ExcelExporter exporter,
                                   CompanyFilterService companyFilterService,
                                   IMediator mediator, ICompanyQueries companyQueries)
        {
            _context = context;
            Exporter = exporter;
            CompanyFilterService = companyFilterService;
            this.mediator = mediator;
            this.companyQueries = companyQueries;
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
            var vm = new CreateCompanyVM()
            {
                Id = id.Value,
                Name = company.Name,
                Site = company.Site,
                Information = company.Information,
                BusinessLineNames = company.BusinessLines.Select(_ => _.BusinessLine.Name).ToList(),
                AffiliationNames = company.Affiliates.Select(_ => _.Region.Name).ToList(),
                Contacts = company.Contacts.Select(_ => new ContactVM()
                {
                    FirstName = _.FirstName,
                    PatronymicName = _.PatronymicName,
                    LastName = _.LastName,
                    Mail = _.Mail,
                    ContactTypeName = _.ContactType.Name
                }).ToList()
            };
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
            var company = await _context.Company.FindAsync(id);
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Export()
        {
            var companies = await companyQueries.GetCompanies(null, null);
            var report = await Exporter.GenerateCompaniesReport(companies);
            return File(report, "application/vnd.ms-excel", "Компании.xslx");
        }

        private void CreateViewDataForChanges()
        {
            ViewData["BusinessLine"] = _context.Set<BusinessLine>();
            ViewData["Regions"] = _context.Set<Region>();
            ViewData["ContactTypes"] = _context.Set<ContactType>();
        }
    }
}
