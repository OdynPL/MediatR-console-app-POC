using FluentValidation;
using PersonManager.Commands;

namespace PersonManager.Validators
{
    public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
    {
        public DeletePersonCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(GlobalConfig.IdPositiveError);
        }
    }
}
