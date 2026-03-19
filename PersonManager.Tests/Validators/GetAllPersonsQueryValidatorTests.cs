using FluentValidation.TestHelper;
using PersonManager.Queries;
using PersonManager.Validators;
using Xunit;

namespace PersonManager.Tests.Validators
{
    public class GetAllPersonsQueryValidatorTests
    {
        private readonly GetAllPersonsQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Empty_Query()
        {
            var model = new GetAllPersonsQuery();
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}