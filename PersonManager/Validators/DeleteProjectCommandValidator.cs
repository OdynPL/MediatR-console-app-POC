using FluentValidation;
using PersonManager.Commands;

namespace PersonManager.Validators
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
