using FluentValidation;
using LisasTours.Application.Commands;

namespace LisasTours.Application.Validations
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(cmd => cmd.CreateCompanyVM.Name).NotEmpty();
        }
    }
}
