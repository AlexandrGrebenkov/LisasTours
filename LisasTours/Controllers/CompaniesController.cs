using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LisasTours.Data;
using LisasTours.Models;
using LisasTours.Models.Base;

namespace LisasTours.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Company
                .Include(c => c.BusinessLines).ThenInclude(bl => bl.BusinessLine)
                .Include(c => c.Affiliates).ThenInclude(a => a.Region);
            return View(await applicationDbContext.ToListAsync());
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
                .Include(c => c.Contacts).ThenInclude(contact => contact.Type)
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
            ViewData["BusinessLine"] = _context.Set<BusinessLine>();
            ViewData["Regions"] = _context.Set<Region>();
            ViewData["RegionId"] = new SelectList(_context.Set<Region>(), "Id", "Name");
            ViewData["ContactTypes"] = new SelectList(_context.Set<ContactType>(), "Id", "Name");
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
                company.BusinessLines = GetNamedCollection<BusinessLine>(businessLines)
                    .Select(_ => new CompanyBusinessLine() { BusinessLine = _ })
                    .ToList();

                company.Affiliates = GetNamedCollection<Region>(affiliations)
                    .Select(_ => new Affiliate() { Region = _ })
                    .ToList();

                company.Contacts.RemoveAll(_ => string.IsNullOrWhiteSpace(_.FirstName) &&
                                        string.IsNullOrWhiteSpace(_.LastName) &&
                                        string.IsNullOrWhiteSpace(_.Mail));
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessLine"] = _context.Set<BusinessLine>();
            ViewData["Regions"] = _context.Set<Region>();
            return View(company);
        }

        IEnumerable<T> GetNamedCollection<T>(IEnumerable<string> names)
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
                if (bb != null) item.Id = bb.Id;
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
                .Include(c => c.Contacts).ThenInclude(contact => contact.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            ViewData["ContactTypes"] = new SelectList(_context.Set<ContactType>(), "Id", "Name");
            ViewData["BusinessLine"] = _context.Set<BusinessLine>();
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
                if (bb != null) b.Id = bb.Id;
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
            ViewData["BusinessLine"] = businessLinesFromDb;
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
                .Include(c => c.Contacts).ThenInclude(contact => contact.Type)
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
    }
}
