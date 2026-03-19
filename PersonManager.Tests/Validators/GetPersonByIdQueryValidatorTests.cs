using FluentValidation.TestHelper;
using PersonManager.Queries;
using PersonManager.Validators;
namespace PersonManager.Tests.Validators
{
    public class GetPersonByIdQueryValidatorTests
    {
        private readonly GetPersonByIdQueryValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Id_Not_Positive()
        {
            var model = new GetPersonByIdQuery(0);
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Id()
        {
            var model = new GetPersonByIdQuery(1);
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}