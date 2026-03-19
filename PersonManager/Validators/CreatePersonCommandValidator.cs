using FluentValidation;
using PersonManager.Commands;

namespace PersonManager.Validators
{
    public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Imię nie może być puste.")
                .MinimumLength(2).WithMessage("Imię musi mieć co najmniej 2 znaki.");
            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("Wiek musi być dodatni.");
        }
    }
}
