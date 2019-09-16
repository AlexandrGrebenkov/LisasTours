﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LisasTours.Application.Commands;
using LisasTours.Data;
using LisasTours.Models;
using LisasTours.Models.Base;
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

        public CompaniesController(ApplicationDbContext context,
                                   ExcelExporter exporter,
                                   CompanyFilterService companyFilterService,
                                   IMediator mediator)
        {
            _context = context;
            Exporter = exporter;
            CompanyFilterService = companyFilterService;
            this.mediator = mediator;
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
        public async Task<IActionResult> Create([Bind("Name,Site,Information,Contacts,BusinessLineNames,AffiliationNames")] CreateCompanyVM vm)
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

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
                                              [Bind("Name,Site,Information,Contacts,BusinessLineNames,AffiliationNames")] CreateCompanyVM vm)
        {
            try
            {
                var result = await mediator.Send(new UpdateCompanyCommand(id, vm));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

            }
            CreateViewDataForChanges();
            return View(vm);
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
