using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using PersonManager.Behaviors;

namespace PersonManager.Tests.Behaviors
{
    public class LoggingBehaviorTests
    {
        public class TestRequest {}

        [Fact]
        public async Task Handle_LogsRequestAndResponse()
        {
            var loggerMock = new Mock<ILogger<LoggingBehavior<TestRequest, string>>>();
            var behavior = new LoggingBehavior<TestRequest, string>(loggerMock.Object);
            var request = new TestRequest();
            var response = "test-response";
            RequestHandlerDelegate<string> next = ct => Task.FromResult(response);

            var result = await behavior.Handle(request, next, CancellationToken.None);

            Assert.Equal(response, result);
        }
    }
}
