using FluentValidation;
using MediatR;
using Moq;
using PersonManager.Behaviors;

namespace PersonManager.Tests.Behaviors
{
    public class ValidationBehaviorTests
    {
        public class TestRequest : IRequest<string> {}

        [Fact]
        public async Task Handle_ThrowsValidationException_WhenValidationFails()
        {
            var validatorMock = new Mock<IValidator<TestRequest>>();
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] {
                    new FluentValidation.Results.ValidationFailure("prop", "error")
                }));
            var behavior = new ValidationBehavior<TestRequest, string>(new[] { validatorMock.Object });
            var request = new TestRequest();
            RequestHandlerDelegate<string> next = ct => Task.FromResult("ok");

            await Assert.ThrowsAsync<ValidationException>(() => behavior.Handle(request, next, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_CallsNext_WhenValidationSucceeds()
        {
            var validatorMock = new Mock<IValidator<TestRequest>>();
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var behavior = new ValidationBehavior<TestRequest, string>(new[] { validatorMock.Object });
            var request = new TestRequest();
            RequestHandlerDelegate<string> next = ct => Task.FromResult("ok");

            var result = await behavior.Handle(request, next, CancellationToken.None);
            Assert.Equal("ok", result);
        }
    }
}
