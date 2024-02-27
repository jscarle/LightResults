using FluentAssertions;
using Xunit;

namespace LightResults.Tests;

public class CustomErrorTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateEmptyCustomError()
    {
        // Arrange
        var error = new CustomError();

        // Act & Assert
        error.Message.Should().BeEmpty();
        error.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void ConstructorWithMessage_ShouldCreateErrorWithMessage()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var error = new CustomError(errorMessage);

        // Assert
        error.Message.Should().Be(errorMessage);
        error.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void ConstructorWithMetadataTuple_ShouldCreateErrorWithSingleMetadata()
    {
        // Arrange
        var metadata = (Key: "Key", Value: "Value");

        // Act
        var error = new CustomError(metadata);

        // Assert
        error.Message.Should().BeEmpty();
        error.Metadata.Should().ContainSingle();
        error.Metadata.First().Key.Should().BeEquivalentTo(metadata.Key);
        error.Metadata.First().Value.Should().BeEquivalentTo(metadata.Value);
    }

    [Fact]
    public void ConstructorWithMetadataDictionary_ShouldCreateErrorWithMultipleMetadata()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "Key1", "Value1" },
            { "Key2", 42 }
        };

        // Act
        var error = new CustomError(metadata);

        // Assert
        error.Message.Should().BeEmpty();
        error.Metadata.Should().HaveCount(2).And.BeEquivalentTo(metadata);
    }

    [Fact]
    public void ConstructorWithMessageAndMetadataTuple_ShouldCreateErrorWithMessageAndSingleMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = (Key: "Key", Value: "Value");

        // Act
        var error = new CustomError(errorMessage, metadata);

        // Assert
        error.Message.Should().Be(errorMessage);
        error.Metadata.Should().ContainSingle();
        error.Metadata.First().Key.Should().BeEquivalentTo(metadata.Key);
        error.Metadata.First().Value.Should().BeEquivalentTo(metadata.Value);
    }

    [Fact]
    public void ConstructorWithMessageAndMetadataDictionary_ShouldCreateErrorWithMessageAndMultipleMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new Dictionary<string, object>
        {
            { "Key1", "Value1" },
            { "Key2", 42 }
        };

        // Act
        var error = new CustomError(errorMessage, metadata);

        // Assert
        error.Message.Should().Be(errorMessage);
        error.Metadata.Should().HaveCount(2).And.BeEquivalentTo(metadata);
    }

    [Theory]
    [InlineData("")]
    [InlineData("An unknown error occured!")]
    public void ToString_ShouldReturnStringRepresentation(string errorMessage)
    {
        // Arrange
        var error = new CustomError(errorMessage);

        // Act & Assert
        error.ToString().Should().Be(errorMessage.Length > 0 ? $"CustomError {{ Message = \"{errorMessage}\" }}" : "CustomError");
    }
}

internal sealed class CustomError : Error
{
    public CustomError()
    {
    }

    public CustomError(string errorMessage) : base(errorMessage)
    {
    }

    public CustomError(string errorMessage, (string Key, object Value) metadata) : base(errorMessage, metadata)
    {
    }

    public CustomError((string Key, object Value) metadata) : base("", metadata)
    {
    }

    public CustomError(string errorMessage, IDictionary<string, object> metadata) : base(errorMessage, metadata)
    {
    }

    public CustomError(IDictionary<string, object> metadata) : base("", metadata)
    {
    }
}