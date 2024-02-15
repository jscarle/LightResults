using FluentAssertions;
using LightResults.Common;
using Xunit;

namespace LightResults.Tests.Common;

public class ResultBaseTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateSuccessResult()
    {
        // Arrange
        var result = new TestResult();

        // Act & Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ConstructorWithErrorObject_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = new TestResult(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().BeEquivalentTo(error);
    }

    [Fact]
    public void ConstructorWithErrorsEnumerable_ShouldCreateFailedResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2")
        };

        // Act
        var result = new TestResult(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().HaveCount(2).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void HasError_WithMatchingErrorType_ShouldReturnTrue()
    {
        // Arrange
        var result = new TestResult(new ValidationError("Validation error"));

        // Act & Assert
        result.HasError<ValidationError>().Should().BeTrue();
    }

    [Fact]
    public void HasError_WithNonMatchingErrorType_ShouldReturnFalse()
    {
        // Arrange
        var result = new TestResult(new Error("Generic error"));

        // Act & Assert
        result.HasError<ValidationError>().Should().BeFalse();
    }

    [Fact]
    public void ToString_WhenSuccess_ShouldReturnProperRepresentation()
    {
        // Arrange
        var result = new TestResult();

        // Act & Assert
        result.ToString().Should().Be("TestResult { IsSuccess = True }");
    }

    [Theory]
    [InlineData("")]
    [InlineData("An unknown error occured!")]
    public void ToString_WhenFailed_ShouldReturnProperRepresentation(string errorMessage)
    {
        // Arrange
        var result = new TestResult(new Error(errorMessage));

        // Act & Assert
        result.ToString().Should().Be(errorMessage.Length > 0 ? $"TestResult {{ IsSuccess = False, Error = \"{errorMessage}\" }}" : "TestResult { IsSuccess = False }");
    }

    private class ValidationError(string errorMessage) : Error(errorMessage);

    private class TestResult : ResultBase
    {
        public TestResult()
        {
        }

        public TestResult(IError error) : base(error)
        {
        }

        public TestResult(IEnumerable<IError> errors) : base(errors)
        {
        }
    }
}
