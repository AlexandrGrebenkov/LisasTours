using LisasTours.Models.ViewModels;
using MediatR;

namespace LisasTours.Application.Commands.Companies
{
    public class CreateCompanyCommand : IRequest<bool>
    {
        public CompanyVM CreateCompanyVM { get; }

        public CreateCompanyCommand(CompanyVM createCompanyVM)
        {
            CreateCompanyVM = createCompanyVM;
        }
    }
}
