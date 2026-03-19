using FluentValidation.TestHelper;
using PersonManager.Commands;
using PersonManager.Validators;

namespace PersonManager.Tests.Validators
{
    public class UpdateProjectCommandValidatorTests
    {
        private readonly UpdateProjectCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            var model = new UpdateProjectCommand { Id = 1, Title = "", MemberIds = new List<int> { 1 } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Have_Error_When_MemberIds_Is_Null_Or_Empty()
        {
            var model = new UpdateProjectCommand { Id = 1, Title = "Test", MemberIds = new List<int>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MemberIds);
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Zero_Or_Negative()
        {
            var model = new UpdateProjectCommand { Id = 0, Title = "Test", MemberIds = new List<int> { 1 } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Model()
        {
            var model = new UpdateProjectCommand { Id = 1, Title = "Valid", MemberIds = new List<int> { 1, 2 } };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}