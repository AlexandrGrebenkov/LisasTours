using LisasTours.Models.ViewModels;
using MediatR;

namespace LisasTours.Application.Commands
{
    public class CreateCompanyCommand : IRequest<bool>
    {
        public CreateCompanyVM CreateCompanyVM { get; }

        public CreateCompanyCommand(CreateCompanyVM createCompanyVM)
        {
            CreateCompanyVM = createCompanyVM;
        }
    }
}
