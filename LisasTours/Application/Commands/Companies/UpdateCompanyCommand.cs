using LisasTours.Models.ViewModels;
using MediatR;

namespace LisasTours.Application.Commands.Companies
{
    public class UpdateCompanyCommand : IRequest<bool>
    {
        public CompanyVM CompanyVM { get; }
        public int Id { get; }

        public UpdateCompanyCommand(int id, CompanyVM companyVM)
        {
            Id = id;
            CompanyVM = companyVM;
        }
    }
}
