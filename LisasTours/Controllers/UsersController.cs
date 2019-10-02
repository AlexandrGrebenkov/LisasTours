using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LisasTours.Application.Queries;
using LisasTours.Models.Identity;
using LisasTours.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LisasTours.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersQueries usersQueries;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public UsersController(IUsersQueries usersQueries,
                               IMapper mapper,
                               IMediator mediator)
        {
            this.usersQueries = usersQueries;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Index()
        {
            var users = mapper.Map<UserVM[]>(await usersQueries.GetAllUsers());
            return View(users);
        }
    }
}
