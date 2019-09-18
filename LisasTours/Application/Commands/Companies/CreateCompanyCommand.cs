using LisasTours.Models.ViewModels;
using MediatR;

namespace LisasTours.Application.Commands.Companies
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
