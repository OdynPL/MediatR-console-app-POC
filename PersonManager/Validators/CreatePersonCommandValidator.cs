using FluentValidation;
using PersonManager.Commands;

namespace PersonManager.Validators
{
    public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(GlobalConfig.PersonNameEmptyError)
                .MinimumLength(2).WithMessage(GlobalConfig.PersonNameMinLengthError);
            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage(GlobalConfig.PersonAgePositiveError);
        }
    }
}
