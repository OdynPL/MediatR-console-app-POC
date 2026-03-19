using FluentValidation;
using MediatRApp.Queries;

namespace MediatRApp.Validators
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
