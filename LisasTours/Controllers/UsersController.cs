using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LisasTours.Application.Queries;
using LisasTours.Data;
using LisasTours.Models.Identity;
using LisasTours.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LisasTours.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public class UsersController : Controller
    {
        private readonly IUsersQueries usersQueries;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IUsersQueries usersQueries,
                               IMapper mapper,
                               IMediator mediator, 
                               ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager)
        {
            this.usersQueries = usersQueries;
            this.mapper = mapper;
            this.mediator = mediator;
            this.context = context;
            this.userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var users = mapper.Map<UserVM[]>(await usersQueries.GetAllUsers());
            return View(users);
        }

        public IActionResult Edit(string id)
        {
            var user = usersQueries.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            var userVm = mapper.Map<UserVM>(user);
            userVm.Roles = userManager.GetRolesAsync(user).Result;
            ViewData["AllRoles"] = context.Set<IdentityRole>();
            return View(userVm);
        }

        [HttpPost]
        public IActionResult Edit(UserVM userVM)
        {

            return RedirectToAction(nameof(Index));
        }
    }
}
