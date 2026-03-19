using FluentValidation;
using PersonManager.Commands;

namespace PersonManager.Validators
{
    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Ulica nie może być pusta.");
            RuleFor(x => x.City)
                .NotEmpty().WithMessage(GlobalConfig.CityEmptyError);
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Kraj nie może być pusty.");
        }
    }
}