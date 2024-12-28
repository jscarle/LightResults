using FluentAssertions;
using Xunit;

namespace LightResults.Tests;

public sealed class ErrorTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateEmptyError()
    {
        // Arrange
        var error = new Error();

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
        var error = new Error(errorMessage);

        // Assert
        error.Message
            .Should()
            .Be(errorMessage);
        error.Metadata
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void ConstructorWithMessageAndMetadataDictionary_ShouldCreateErrorWithMessageAndMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new Dictionary<string, object>
        {
            { "Key1", "Value1" },
            { "Key2", 42 },
        };

        // Act
        var error = new Error(errorMessage, metadata);

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

    [Fact]
    public void MessagePropertyInit_ShouldCreateErrorWithMessage()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var error = new Error
        {
            Message = errorMessage,
        };

        // Assert
        error.Message
            .Should()
            .Be(errorMessage);
        error.Metadata
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void MetadataPropertyInit_ShouldCreateErrorWithMetadata()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "Key1", "Value1" },
            { "Key2", 42 },
        };
        var error = new Error
        {
            Metadata = metadata,
        };

        // Assert
        error.Message
            .Should()
            .BeEmpty();
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
        var error = new Error(errorMessage);

        // Assert
        error.ToString()
            .Should()
            .Be(errorMessage.Length > 0 ? $"Error {{ Message = \"{errorMessage}\" }}" : "Error");
    }
}
