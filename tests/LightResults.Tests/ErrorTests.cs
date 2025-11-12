using Shouldly;
using System.Diagnostics.CodeAnalysis;

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

    [Fact]
    public void ConstructorWithException_ShouldCreateErrorWithMessageAndMetadata()
    {
        // Arrange
        var exception = new InvalidOperationException("ex message");

        // Act
        var error = new Error(exception);

        // Assert
        error.Message.ShouldBe($"{exception.GetType().Name}: {exception.Message}");
        error.Metadata.Count.ShouldBe(1);
        var metadata = error.Metadata.Single();
        metadata.Key.ShouldBe("Exception");
        metadata.Value.ShouldBe(exception);
    }

    [Fact]
    public void ConstructorWithMessageAndException_ShouldCreateErrorWithMessageAndMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var exception = new InvalidOperationException("ex message");

        // Act
        var error = new Error(errorMessage, exception);

        // Assert
        error.Message.ShouldBe(errorMessage);
        error.Metadata.Count.ShouldBe(1);
        var metadata = error.Metadata.Single();
        metadata.Key.ShouldBe("Exception");
        metadata.Value.ShouldBe(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData("An unknown error occurred!")]
    public void ToString_ShouldReturnStringRepresentation(string errorMessage)
    {
        // Arrange
        var error = new Error(errorMessage);

        // Assert
        error.ToString()
            .ShouldBe(errorMessage.Length > 0 ? $"Error {{ Message = \"{errorMessage}\" }}" : "Error");
    }

    [Fact]
    public void Equals_Error_ShouldReturnTrueForEqualErrors()
    {
        // Arrange
        var error1 = new Error("error", ("Key", 1));
        var error2 = new Error("error", ("Key", 1));

        // Assert
        error1.Equals(error2)
            .ShouldBeTrue();
    }

    [Fact]
    public void Equals_Error_ShouldReturnFalseForUnequalErrors()
    {
        // Arrange
        var error1 = new Error("error", ("Key", 1));
        var error2 = new Error("error", ("Key", 2));

        // Assert
        error1.Equals(error2)
            .ShouldBeFalse();
    }

    [Fact]
    public void Equals_Object_ShouldReturnTrueForEqualErrors()
    {
        // Arrange
        var error1 = new Error("error", ("Key", 1));
        var error2 = new Error("error", ("Key", 1));

        // Assert
        error1.Equals((object)error2)
            .ShouldBeTrue();
    }

    [Fact]
    public void Equals_Object_ShouldReturnFalseForUnequalErrors()
    {
        // Arrange
        var error1 = new Error("error", ("Key", 1));
        var error2 = new Error("error", ("Key", 2));

        // Assert
        error1.Equals((object)error2)
            .ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualErrors()
    {
        // Arrange
        var error1 = new Error("error", ("Key", 1));
        var error2 = new Error("error", ("Key", 1));

        // Assert
        error1.GetHashCode()
            .ShouldBe(error2.GetHashCode());
    }

    [Fact]
    public void op_Equality_Error_ShouldReturnTrueForEqualErrors()
    {
        // Arrange
        var error1 = new Error("error", ("Key", 1));
        var error2 = new Error("error", ("Key", 1));

        // Assert
        (error1 == error2).ShouldBeTrue();
    }

    [Fact]
    public void op_Equality_Error_ShouldReturnFalseForUnequalErrors()
    {
        // Arrange
        var error1 = new Error("error", ("Key", 1));
        var error2 = new Error("error", ("Key", 2));

        // Assert
        (error1 == error2).ShouldBeFalse();
    }

    [Fact]
    public void op_Inequality_Error_ShouldReturnFalseForEqualErrors()
    {
        // Arrange
        var error1 = new Error("error", ("Key", 1));
        var error2 = new Error("error", ("Key", 1));

        // Assert
        (error1 != error2).ShouldBeFalse();
    }

    [Fact]
    public void op_Inequality_Error_ShouldReturnTrueForUnequalErrors()
    {
        // Arrange
        var error1 = new Error("error", ("Key", 1));
        var error2 = new Error("error", ("Key", 2));

        // Assert
        (error1 != error2).ShouldBeTrue();
    }

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
