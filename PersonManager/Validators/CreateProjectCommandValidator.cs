using FluentValidation;
using PersonManager.Commands;

namespace PersonManager.Validators
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.MemberIds).NotNull().Must(x => x.Count > 0).WithMessage("Projekt musi mieć co najmniej jednego członka");
        }
    }
}
