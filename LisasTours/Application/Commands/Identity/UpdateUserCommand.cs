using LisasTours.Models.ViewModels;
using MediatR;

namespace LisasTours.Application.Commands.Identity
{
    public class UpdateUserCommand : IRequest
    {
        public UserVM UserVM { get; }

        public UpdateUserCommand(UserVM userVM)
        {
            UserVM = userVM;
        }
    }
}
