using Shouldly;

namespace LightResults.Tests;

public sealed class CustomErrorTests
{
    [Test]
    public void DefaultConstructor_ShouldCreateEmptyCustomError()
    {
        // Arrange
        var error = new CustomError();

        // Assert
        error.Message.ShouldBeEmpty();
        error.Metadata.ShouldBeEmpty();
    }

    [Test]
    public void ConstructorWithMessage_ShouldCreateErrorWithMessage()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var error = new CustomError(errorMessage);

        // Assert
        error.Message.ShouldBe(errorMessage);
        error.Metadata.ShouldBeEmpty();
    }

    [Test]
    public void ConstructorWithMessageAndMetadataDictionary_ShouldCreateErrorWithMessageAndMultipleMetadata()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new Dictionary<string, object?>
        {
            { "Key1", "Value1" },
            { "Key2", 42 },
        };

        // Act
        var error = new CustomError(errorMessage, metadata);

        // Assert
        error.Message.ShouldBe(errorMessage);
        error.Metadata.Count.ShouldBe(2);
        error.Metadata.ShouldBe(metadata);
    }

    [Test]
    [Arguments("")]
    [Arguments("An unknown error occurred!")]
    public void ToString_ShouldReturnStringRepresentation(string errorMessage)
    {
        // Arrange
        var error = new CustomError(errorMessage);

        // Assert
        error.ToString().ShouldBe(errorMessage.Length > 0
            ? $"CustomError {{ Message = \"{errorMessage}\" }}"
            : "CustomError");
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

        public CustomError(string errorMessage, IReadOnlyDictionary<string, object?> metadata)
            : base(errorMessage, metadata)
        {
        }
    }
}
