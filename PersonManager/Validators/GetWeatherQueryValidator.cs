using FluentValidation;
using PersonManager.Queries;

namespace PersonManager.Validators
{
    public class GetWeatherQueryValidator : AbstractValidator<GetWeatherQuery>
    {
        public GetWeatherQueryValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Miasto nie może być puste.")
                .MinimumLength(2).WithMessage("Miasto musi mieć co najmniej 2 znaki.");
        }
    }
}
