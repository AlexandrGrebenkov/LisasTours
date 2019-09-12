using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LisasTours.Data;
using LisasTours.Models;
using LisasTours.Models.Base;
using LisasTours.Models.ViewModels;
using LisasTours.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LisasTours.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ExcelExporter Exporter;
        private readonly CompanyFilterService CompanyFilterService;

        public CompaniesController(ApplicationDbContext context, ExcelExporter exporter, CompanyFilterService companyFilterService)
        {
            _context = context;
            Exporter = exporter;
            CompanyFilterService = companyFilterService;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Company
                .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                .Include(c => c.Affiliates).ThenInclude(a => a.Region);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Search(CompanySearchVM searchVM)
        {
            var companies = await _context.Company
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Where(CompanyFilterService.CreateCompanyFilterExpression(searchVM))
                .ToListAsync();
            return View(nameof(Index), companies);
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Include(c => c.Contacts).ThenInclude(contact => contact.ContactType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            CreateViewDataForChanges();
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Site,Information,Contacts")] Company company,
                                                [Bind(Prefix = "BusinessLine")] string[] businessLines,
                                                [Bind(Prefix = "Affiliation")] string[] affiliations)
        {
            if (ModelState.IsValid)
            {
                company.Name?.Trim();
                company.Site = company.Site?.Trim().Replace("http://", "").Replace("https://", "");
                company.Information?.Trim();
                foreach (var contact in company.Contacts)
                {
                    contact.FirstName?.Trim();
                    contact.LastName?.Trim();
                    contact.PatronymicName?.Trim();
                    contact.Mail?.Trim();
                }

                company.BusinessLines = GetNamedCollection<BusinessLine>(businessLines)
                    .Select(_ => new CompanyBusinessLine() { BusinessLine = _ })
                    .ToList();

                company.Affiliates = GetNamedCollection<Region>(affiliations)
                    .Select(_ => new Affiliate() { Region = _ })
                    .ToList();

                company.Contacts.RemoveAll(_ => string.IsNullOrWhiteSpace(_.FirstName) &&
                                        string.IsNullOrWhiteSpace(_.LastName) &&
                                        string.IsNullOrWhiteSpace(_.Mail));
                var types = GetNamedCollection<ContactType>(company.Contacts.Select(_ => _.ContactType.Name))
                    .ToList();
                foreach (var contact in company.Contacts)
                {
                    contact.ContactType = types.First(_=>_.Name == contact.ContactType.Name);
                }

                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CreateViewDataForChanges();
            return View(company);
        }

        private IEnumerable<T> GetNamedCollection<T>(IEnumerable<string> names)
            where T : NamedEntity, new()
        {
            var newCollection = names
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(str => new T() { Name = str })
                .ToList();
            var collectionFromDb = _context.Set<T>().ToList();

            // колхоз для получения Id по стоке названия
            foreach (var item in newCollection)
            {
                var bb = collectionFromDb.FirstOrDefault(_ => _.Name == item.Name);
                if (bb != null)
                {
                    item.Id = bb.Id;
                }
            }
            return newCollection;
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Include(c => c.Contacts).ThenInclude(contact => contact.ContactType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            CreateViewDataForChanges();
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
                                              [Bind("Id,Name,Site,Information,Contacts")] Company company,
                                              [Bind(Prefix = "BusinessLine")] string[] businessLines)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            var bl = businessLines.Select(str => new BusinessLine() { Name = str }).ToList();
            var businessLinesFromDb = _context.Set<BusinessLine>().ToList();

            // колхоз для получения Id BusinessLine по стоке названия
            foreach (var b in bl)
            {
                var bb = businessLinesFromDb.FirstOrDefault(_ => _.Name == b.Name);
                if (bb != null)
                {
                    b.Id = bb.Id;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            CreateViewDataForChanges();
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Include(c => c.Contacts).ThenInclude(contact => contact.ContactType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Company.FindAsync(id);
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Export()
        {
            var companies = await _context.Company
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                .Include(c => c.Affiliates).ThenInclude(a => a.Region)
                .Include(c => c.Contacts).ThenInclude(c => c.ContactType)
                //.Where(CreateFilterExpression(searchVM))
                .ToListAsync();
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
