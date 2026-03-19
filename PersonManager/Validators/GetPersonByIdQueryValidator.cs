using FluentValidation;
using PersonManager.Queries;

namespace PersonManager.Validators
{
    public class GetPersonByIdQueryValidator : AbstractValidator<GetPersonByIdQuery>
    {
        public GetPersonByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID musi być dodatnie.");
        }
    }
}
