using FluentValidation.TestHelper;
using PersonManager.Commands;
using PersonManager.Validators;
using Xunit;

namespace PersonManager.Tests.Validators
{
    public class CreateAddressCommandValidatorTests
    {
        private readonly CreateAddressCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Street_Empty()
        {
            var model = new CreateAddressCommand { Street = "", City = "Miasto", Country = "PL" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Street);
        }

        [Fact]
        public void Should_Have_Error_When_City_Empty()
        {
            var model = new CreateAddressCommand { Street = "Ulica", City = "", Country = "PL" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Fact]
        public void Should_Have_Error_When_Country_Empty()
        {
            var model = new CreateAddressCommand { Street = "Ulica", City = "Miasto", Country = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Country);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Model()
        {
            var model = new CreateAddressCommand { Street = "Ulica", City = "Miasto", Country = "PL" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}