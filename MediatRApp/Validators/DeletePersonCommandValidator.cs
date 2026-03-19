using FluentValidation;
using MediatRApp.Commands;

namespace MediatRApp.Validators
{
    public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
    {
        public DeletePersonCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID musi być dodatnie.");
        }
    }
}
