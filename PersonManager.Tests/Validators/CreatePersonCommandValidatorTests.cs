using FluentValidation.TestHelper;
using PersonManager.Commands;
using PersonManager.Validators;

namespace PersonManager.Tests.Validators
{
    public class CreatePersonCommandValidatorTests
    {
        private readonly CreatePersonCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Name_Empty_Or_TooShort()
        {
            var model1 = new CreatePersonCommand("", 20);
            var result1 = _validator.TestValidate(model1);
            result1.ShouldHaveValidationErrorFor(x => x.Name);

            var model2 = new CreatePersonCommand("A", 20);
            var result2 = _validator.TestValidate(model2);
            result2.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Age_Not_Positive()
        {
            var model = new CreatePersonCommand("Jan", 0);
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Age);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Model()
        {
            var model = new CreatePersonCommand("Jan", 30);
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}