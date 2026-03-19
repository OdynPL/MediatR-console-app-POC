using FluentValidation.TestHelper;
using PersonManager.Commands;
using PersonManager.Validators;
using Xunit;
using System.Collections.Generic;

namespace PersonManager.Tests.Validators
{
    public class CreateProjectCommandValidatorTests
    {
        private readonly CreateProjectCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            var model = new CreateProjectCommand { Title = "", MemberIds = new List<int> { 1 } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Have_Error_When_MemberIds_Is_Null_Or_Empty()
        {
            var model = new CreateProjectCommand { Title = "Test", MemberIds = new List<int>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MemberIds);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Model()
        {
            var model = new CreateProjectCommand { Title = "Valid", MemberIds = new List<int> { 1, 2 } };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}