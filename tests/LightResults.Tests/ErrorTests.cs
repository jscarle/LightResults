using Shouldly;
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
        error.Message.ShouldBeEmpty();
        error.Metadata.ShouldBeEmpty();
    }

    [Fact]
    public void ConstructorWithMessage_ShouldCreateErrorWithMessage()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var error = new Error(errorMessage);

        // Assert
        error.Message.ShouldBe(errorMessage);
        error.Metadata.ShouldBeEmpty();
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
        error.Message.ShouldBe(errorMessage);
        error.Metadata.Count.ShouldBe(2);
        error.Metadata.ShouldBe(metadata);
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
        error.Message.ShouldBe(errorMessage);
        error.Metadata.ShouldBeEmpty();
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
        error.Message.ShouldBeEmpty();
        error.Metadata.Count.ShouldBe(2);
        error.Metadata.ShouldBe(metadata);
    }

    [Theory]
    [InlineData("")]
    [InlineData("An unknown error occured!")]
    public void ToString_ShouldReturnStringRepresentation(string errorMessage)
    {
        // Arrange
        var error = new Error(errorMessage);

        // Assert
        error.ToString().ShouldBe(errorMessage.Length > 0 ? $"Error {{ Message = \"{errorMessage}\" }}" : "Error");
    }
}