using LisasTours.Models.ViewModels;
using MediatR;

namespace LisasTours.Application.Commands
{
    public class UpdateCompanyCommand : IRequest<bool>
    {
        public CreateCompanyVM CreateCompanyVM { get; }
        public int Id { get; }

        public UpdateCompanyCommand(int id, CreateCompanyVM createCompanyVM)
        {
            Id = id;
            CreateCompanyVM = createCompanyVM;
        }
    }
}
