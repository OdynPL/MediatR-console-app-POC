using FluentValidation;
using MediatRApp.Queries;

namespace MediatRApp.Validators
{
    public class GetAllPersonsQueryValidator : AbstractValidator<GetAllPersonsQuery>
    {
        public GetAllPersonsQueryValidator()
        {
            // Brak właściwości do walidacji, klasa pusta
        }
    }
}
