using FluentValidation.TestHelper;
using PersonManager.Queries;
using PersonManager.Validators;
namespace PersonManager.Tests.Validators
{
    public class GetWeatherQueryValidatorTests
    {
        private readonly GetWeatherQueryValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_City_Empty_Or_TooShort()
        {
            var model1 = new GetWeatherQuery("");
            var result1 = _validator.TestValidate(model1);
            result1.ShouldHaveValidationErrorFor(x => x.City);

            var model2 = new GetWeatherQuery("A");
            var result2 = _validator.TestValidate(model2);
            result2.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_City()
        {
            var model = new GetWeatherQuery("Warszawa");
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}