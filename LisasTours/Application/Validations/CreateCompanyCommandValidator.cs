using FluentValidation;
using LisasTours.Application.Commands.Companies;

namespace LisasTours.Application.Validations
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(cmd => cmd.CreateCompanyVM.Name)
                .NotEmpty()
                .WithMessage("Название компании не может быть пустым");
        }
    }
}
