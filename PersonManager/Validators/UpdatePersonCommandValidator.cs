using FluentValidation;
using PersonManager.Commands;

namespace PersonManager.Validators
{
    public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(GlobalConfig.IdPositiveError);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(GlobalConfig.PersonNameEmptyError)
                .MinimumLength(2).WithMessage(GlobalConfig.PersonNameMinLengthError);
            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage(GlobalConfig.PersonAgePositiveError);
        }
    }
}
