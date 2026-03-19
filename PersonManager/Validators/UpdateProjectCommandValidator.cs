using FluentValidation;
using PersonManager.Commands;

namespace PersonManager.Validators
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.MemberIds).NotNull().Must(x => x.Count > 0).WithMessage("Projekt musi mieć co najmniej jednego członka");
        }
    }
}
