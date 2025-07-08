#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using Shouldly;

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
    public void ConstructorWithMessageAndMetadataTuple_ShouldCreateErrorWithMessageAndMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = (Key: "Key1", Value: "Value1");

        // Act
        var error = new Error(errorMessage, metadata);

        // Assert
        error.Message.ShouldBe(errorMessage);
        error.Metadata.Count.ShouldBe(1);
        var firstMetadata = error.Metadata.First();
        firstMetadata.Key.ShouldBe(metadata.Key);
        firstMetadata.Value.ShouldBe(metadata.Value);
    }

    [Fact]
    public void ConstructorWithMessageAndMetadataKeyValuePair_ShouldCreateErrorWithMessageAndMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new KeyValuePair<string, object?>("Key1", "Value1");

        // Act
        var error = new Error(errorMessage, metadata);

        // Assert
        error.Message.ShouldBe(errorMessage);
        error.Metadata.Count.ShouldBe(1);
        var firstMetadata = error.Metadata.First();
        firstMetadata.Key.ShouldBe(metadata.Key);
        firstMetadata.Value.ShouldBe(metadata.Value);
    }

#if NET6_0_OR_GREATER
    [Fact]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public void ConstructorWithMessageAndMetadataIEnumerable_ShouldCreateErrorWithMessageAndMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new Dictionary<string, object?>
        {
            { "Key1", "Value1" },
            { "Key2", 42 },
        }.AsEnumerable();

        // Act
        var error = new Error(errorMessage, metadata);

        // Assert
        error.Message.ShouldBe(errorMessage);
        error.Metadata.Count.ShouldBe(2);
        error.Metadata.ShouldBe(metadata);
    }
#endif

    [Fact]
    public void ConstructorWithMessageAndMetadataDictionary_ShouldCreateErrorWithMessageAndMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new Dictionary<string, object?>
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
        var metadata = new Dictionary<string, object?>
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
    [InlineData("An unknown error occurred!")]
    public void ToString_ShouldReturnStringRepresentation(string errorMessage)
    {
        // Arrange
        var error = new Error(errorMessage);

        // Assert
        error.ToString().ShouldBe(errorMessage.Length > 0 ? $"Error {{ Message = \"{errorMessage}\" }}" : "Error");    }

    [Fact]
    public void ExceptionProperty_ShouldReturnExceptionWhenMetadataContainsException()
    {
        // Arrange
        var exception = new InvalidOperationException();
        var error = new Error("", ("Exception", exception));

        // Assert
        error.Exception.ShouldBe(exception);
    }

    [Fact]
    public void ExceptionProperty_ShouldReturnNullWhenMetadataDoesNotContainException()
    {
        // Arrange
        var error = new Error();

        // Assert
        error.Exception.ShouldBeNull();
    }

    [Fact]
    public void ExceptionProperty_ShouldReturnNullWhenMetadataIsNotException()
    {
        // Arrange
        var error = new Error("", ("Exception", "not exception"));

        // Assert
        error.Exception.ShouldBeNull();
    }
}
