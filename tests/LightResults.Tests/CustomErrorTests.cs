using FluentAssertions;
using Xunit;

namespace LightResults.Tests;

public sealed class CustomErrorTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateEmptyCustomError()
    {
        // Arrange
        var error = new CustomError();

        // Assert
        error.Message
            .Should()
            .BeEmpty();
        error.Metadata
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void ConstructorWithMessage_ShouldCreateErrorWithMessage()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var error = new CustomError(errorMessage);

        // Assert
        error.Message
            .Should()
            .Be(errorMessage);
        error.Metadata
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void ConstructorWithMessageAndMetadataDictionary_ShouldCreateErrorWithMessageAndMultipleMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new Dictionary<string, object>
        {
            { "Key1", "Value1" },
            { "Key2", 42 },
        };

        // Act
        var error = new CustomError(errorMessage, metadata);

        // Assert
        error.Message
            .Should()
            .Be(errorMessage);
        error.Metadata
            .Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo(metadata);
    }

    [Theory]
    [InlineData("")]
    [InlineData("An unknown error occured!")]
    public void ToString_ShouldReturnStringRepresentation(string errorMessage)
    {
        // Arrange
        var error = new CustomError(errorMessage);

        // Assert
        error.ToString()
            .Should()
            .Be(errorMessage.Length > 0 ? $"CustomError {{ Message = \"{errorMessage}\" }}" : "CustomError");
    }

    private sealed class CustomError : Error
    {
        public CustomError()
        {
        }

        public CustomError(string errorMessage)
            : base(errorMessage)
        {
        }

        public CustomError(string errorMessage, IReadOnlyDictionary<string, object> metadata)
            : base(errorMessage, metadata)
        {
        }
    }
}
