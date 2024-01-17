using FluentAssertions;
using Xunit;

namespace LightResults.Tests;

public class ErrorTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateEmptyError()
    {
        // Arrange
        var error = new Error();

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
        var error = new Error(errorMessage);

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
        var error = new Error(metadata);

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
        var error = new Error(metadata);

        // Assert
        error.Message.Should().BeEmpty();
        error.Metadata.Should().HaveCount(2)
            .And.BeEquivalentTo(metadata);
    }

    [Fact]
    public void ConstructorWithMessageAndMetadataTuple_ShouldCreateErrorWithMessageAndSingleMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = (Key: "Key", Value: "Value");

        // Act
        var error = new Error(errorMessage, metadata);

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
        var error = new Error(errorMessage, metadata);

        // Assert
        error.Message.Should().Be(errorMessage);
        error.Metadata.Should().HaveCount(2)
            .And.BeEquivalentTo(metadata);
    }

    [Fact]
    public void ToString_ShouldReturnMessageIfNotEmpty()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var error = new Error(errorMessage);

        // Act & Assert
        error.ToString().Should().Be(errorMessage);
    }

    [Fact]
    public void ToString_ShouldReturnBaseToStringIfMessageIsEmpty()
    {
        // Arrange
        var error = new Error();

        // Act & Assert
        error.ToString().Should().Be(typeof(Error).FullName);
    }
}