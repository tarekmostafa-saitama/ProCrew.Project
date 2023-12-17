using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using ProCrew.Application.Common.Behaviours;
using Xunit;
namespace ProCrew.ApplicationUnitTests.Common.Behaviours
{
    public class ValidationBehaviourTests
    {
        [Fact]
        public async Task Handle_WithValidRequest_ShouldNotThrowException()
        {
            // Arrange
            var validators = new List<IValidator<TestRequest>>();
            var validationBehaviour = new ValidationBehaviour<TestRequest, TestResponse>(validators);
            var request = new TestRequest();
            var next = new Mock<RequestHandlerDelegate<TestResponse>>();
            // Act
            Func<Task> act = async () => await validationBehaviour.Handle(request, next.Object, CancellationToken.None);
            // Assert
            await act.Should().NotThrowAsync<Application.Common.Exceptions.ValidationException>();
        }
        [Fact]
        public async Task Handle_WithInvalidRequest_ShouldThrowValidationException()
        {
            // Arrange
            var validatorMock = new Mock<IValidator<TestRequest>>();
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "Error") }));
            var validators = new List<IValidator<TestRequest>> { validatorMock.Object };
            var validationBehaviour = new ValidationBehaviour<TestRequest, TestResponse>(validators);
            var request = new TestRequest();
            var next = new Mock<RequestHandlerDelegate<TestResponse>>();
            // Act & Assert
            var act = () => validationBehaviour.Handle(request, next.Object, CancellationToken.None);
            await act.Should().ThrowAsync<Application.Common.Exceptions.ValidationException>();
            next.Verify(x => x(), Times.Never);
        }
        public class TestRequest : IRequest<TestResponse> { }
        public class TestResponse { }
    }
}
