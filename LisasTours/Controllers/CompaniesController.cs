using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LisasTours.Data;
using LisasTours.Models;

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
            var applicationDbContext = _context.Company.Include(c => c.BusinessLine).Include(c => c.Region);
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
                .Include(c => c.BusinessLine)
                .Include(c => c.Region)
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
            ViewData["BusinessLineId"] = new SelectList(_context.Set<BusinessLine>(), "Id", "Name");
            ViewData["RegionId"] = new SelectList(_context.Set<Region>(), "Id", "Name");
            ViewData["ContactTypes"] = new SelectList(_context.Set<ContactType>(), "Id", "Name");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Site,Information,BusinessLineId,RegionId")] Company company, List<Contact> contacts)
        {
            if (ModelState.IsValid)
            {
                contacts.RemoveAll(_ => string.IsNullOrWhiteSpace(_.FirstName) &&
                                        string.IsNullOrWhiteSpace(_.LastName) &&
                                        string.IsNullOrWhiteSpace(_.Mail));

                company.Contacts = contacts;
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessLineId"] = new SelectList(_context.Set<BusinessLine>(), "Id", "Name", company.BusinessLineId);            
            ViewData["RegionId"] = new SelectList(_context.Set<Region>(), "Id", "Name", company.RegionId);
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .Include(c => c.BusinessLine)
                .Include(c => c.Region)
                .Include(c => c.Contacts).ThenInclude(contact => contact.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            ViewData["BusinessLineId"] = new SelectList(_context.Set<BusinessLine>(), "Id", "Name", company.BusinessLineId);
            ViewData["RegionId"] = new SelectList(_context.Set<Region>(), "Id", "Name", company.RegionId);
            ViewData["ContactTypes"] = new SelectList(_context.Set<ContactType>(), "Id", "Name");
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Site,Information,BusinessLineId,RegionId")] Company company, List<Contact> contacts)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contacts.RemoveAll(_ => string.IsNullOrWhiteSpace(_.FirstName) &&
                                            string.IsNullOrWhiteSpace(_.LastName) &&
                                            string.IsNullOrWhiteSpace(_.Mail));

                    company.Contacts = contacts;
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
            ViewData["BusinessLineId"] = new SelectList(_context.Set<BusinessLine>(), "Id", "Name", company.BusinessLineId);
            ViewData["RegionId"] = new SelectList(_context.Set<Region>(), "Id", "Name", company.RegionId);
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
                .Include(c => c.BusinessLine)
                .Include(c => c.Region)
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
