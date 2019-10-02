using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LisasTours.Data;
using LisasTours.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LisasTours.Application.Commands.Identity
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UpdateUserCommandHandler(ApplicationDbContext context,
                                        UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = context.Users.Find(request.UserVM.Id);
            if (user != null)
            {
                var roles = request.UserVM.Roles;

                var userRoles = await userManager.GetRolesAsync(user);
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await userManager.AddToRolesAsync(user, addedRoles);
                await userManager.RemoveFromRolesAsync(user, removedRoles);
            }

            return new Unit();
        }
    }
}
