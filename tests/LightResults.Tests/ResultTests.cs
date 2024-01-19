using FluentAssertions;
using Xunit;

namespace LightResults.Tests;

public class ResultTests
{
    [Fact]
    public void Ok_ShouldCreateSuccessResult()
    {
        // Act
        var result = Result.Ok();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void OkTValue_WithValue_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result.Ok(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Should().BeEmpty();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Fail_ShouldCreateFailedResultWithSingleError()
    {
        // Act
        var result = Result.Fail();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("");
    }

    [Fact]
    public void FailTValue_ShouldCreateFailedResultWithSingleError()
    {
        // Act
        var result = Result.Fail<object>();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("");
    }

    [Fact]
    public void Fail_WithErrorMessage_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Fail(errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be(errorMessage);
    }

    [Fact]
    public void FailTValue_WithErrorMessage_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Fail<object>(errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be(errorMessage);
    }

    [Fact]
    public void Fail_WithErrorObject_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Fail(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(error);
    }

    [Fact]
    public void FailTValue_WithErrorObject_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Fail<object>(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(error);
    }

    [Fact]
    public void Fail_WithErrorsEnumerable_ShouldCreateFailedResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2")
        };

        // Act
        var result = Result.Fail(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().HaveCount(2)
            .And.BeEquivalentTo(errors);
    }

    [Fact]
    public void FailTValue_WithErrorsEnumerable_ShouldCreateFailedResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2")
        };

        // Act
        var result = Result.Fail<object>(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().HaveCount(2)
            .And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ToString_ShouldReturnStringRepresentation()
    {
        // Arrange
        var result = Result.Ok();

        // Act & Assert
        result.ToString().Should().Be("Result { IsSuccess = True }");
    }
}
