using FluentAssertions;
using Xunit;

namespace LightResults.Tests;

public class ResultTValueTests
{
    [Fact]
    public void Value_WhenResultIsSuccess_ShouldReturnAssignedValue()
    {
        // Arrange
        var result = Result<int>.Ok(42);

        // Act & Assert
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Value_WhenResultIsFailed_ShouldThrowException()
    {
        // Arrange
        var result = Result<int>.Fail("Error message");

        // Act & Assert
        result.Invoking(r => _ = r.Value).Should().Throw<InvalidOperationException>()
            .WithMessage("Result is failed. Value is not set.");
    }

    [Fact]
    public void Ok_WithValue_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result<int>.Ok(value);

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
        var result = Result<int>.Fail();

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
        var result = Result<int>.Fail(errorMessage);

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
        var result = Result<int>.Fail(error);

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
        var result = Result<int>.Fail(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().HaveCount(2)
            .And.BeEquivalentTo(errors);
    }
}
