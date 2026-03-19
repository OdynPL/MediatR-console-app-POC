using FluentValidation.TestHelper;
using PersonManager.Commands;
using PersonManager.Validators;
using Xunit;

namespace PersonManager.Tests.Validators
{
    public class DeletePersonCommandValidatorTests
    {
        private readonly DeletePersonCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Id_Not_Positive()
        {
            var model = new DeletePersonCommand { Id = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Id()
        {
            var model = new DeletePersonCommand { Id = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}