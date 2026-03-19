using FluentValidation;
using PersonManager.Queries;

namespace PersonManager.Validators
{
    public class GetAllPersonsQueryValidator : AbstractValidator<GetAllPersonsQuery>
    {
        public GetAllPersonsQueryValidator()
        {
            // Brak właściwości do walidacji, klasa pusta
        }
    }
}
