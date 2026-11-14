using Shouldly;

namespace LightResults.Tests;

public sealed class SpanFactoryTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Failure_WithErrorSpan_ShouldSnapshotErrorsInOrder(int count)
    {
        // Arrange
        var errors = new IError[count];
        for (var index = 0; index < count; index++)
            errors[index] = new Error($"Error {index}");
        var expected = errors.ToArray();

        // Act
        var result = Result.Failure(errors.AsSpan());
        Array.Clear(errors);

        // Assert
        result.IsFailure().ShouldBeTrue();
        result.Errors.ShouldBe(expected);
        if (count == 0)
        {
            result.IsFailure(out var error).ShouldBeTrue();
            error.ShouldBeSameAs(Error.Empty);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void FailureTValue_WithErrorSpan_ShouldSnapshotErrorsInOrder(int count)
    {
        // Arrange
        var errors = new IError[count];
        for (var index = 0; index < count; index++)
            errors[index] = new Error($"Error {index}");
        var expected = errors.ToArray();

        // Act
        var result = Result.Failure<int>(errors.AsSpan());
        Array.Clear(errors);

        // Assert
        result.IsFailure().ShouldBeTrue();
        result.IsSuccess(out var value).ShouldBeFalse();
        value.ShouldBe(0);
        result.Errors.ShouldBe(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Error_WithMetadataSpan_ShouldSnapshotEntries(int count)
    {
        // Arrange
        var metadata = new KeyValuePair<string, object?>[count];
        for (var index = 0; index < count; index++)
            metadata[index] = new($"Key{index}", $"Value{index}");

        // Act
        var error = new Error("Failure", metadata.AsSpan());
        Array.Clear(metadata);

        // Assert
        error.Message.ShouldBe("Failure");
        error.Metadata.Count.ShouldBe(count);
        for (var index = 0; index < count; index++)
            error.Metadata[$"Key{index}"].ShouldBe($"Value{index}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Failure_WithMetadataSpan_ShouldSnapshotEntriesForBothResultTypes(int count)
    {
        // Arrange
        var metadata = new KeyValuePair<string, object?>[count];
        for (var index = 0; index < count; index++)
            metadata[index] = new($"Key{index}", $"Value{index}");

        // Act
        var result = Result.Failure("Failure", metadata.AsSpan());
        var valueResult = Result.Failure<int>("Failure", metadata.AsSpan());
        Array.Clear(metadata);

        // Assert
        result.IsFailure(out var error).ShouldBeTrue();
        valueResult.IsFailure(out var valueError).ShouldBeTrue();
        error.Message.ShouldBe("Failure");
        valueError.Message.ShouldBe("Failure");
        error.Metadata.Count.ShouldBe(count);
        valueError.Metadata.Count.ShouldBe(count);
        for (var index = 0; index < count; index++)
        {
            error.Metadata[$"Key{index}"].ShouldBe($"Value{index}");
            valueError.Metadata[$"Key{index}"].ShouldBe($"Value{index}");
        }
    }

    [Fact]
    public void SpanMetadata_WithDuplicateOrNullKeys_ShouldRejectInvalidEntries()
    {
        // Arrange
        KeyValuePair<string, object?>[] duplicateKeys = [new("Key", "first"), new("Key", "second")];
        KeyValuePair<string, object?>[] nullKey = [default];

        // Act & Assert
        Should.Throw<ArgumentException>(() => new Error("Failure", duplicateKeys.AsSpan()));
        Should.Throw<ArgumentNullException>(() => new Error("Failure", nullKey.AsSpan()));
        Should.Throw<ArgumentException>(() => Result.Failure("Failure", duplicateKeys.AsSpan()));
        Should.Throw<ArgumentException>(() => Result.Failure<int>("Failure", duplicateKeys.AsSpan()));
    }

    [Fact]
    public void SpanMetadata_WithOrdinalKeysAndException_ShouldPreserveMetadataContract()
    {
        // Arrange
        var exception = new InvalidOperationException("Failure");
        KeyValuePair<string, object?>[] metadata = [new("Key", null), new("key", "value"), new("Exception", exception)];

        // Act
        var result = Result.Failure("Failure", metadata.AsSpan());

        // Assert
        result.IsFailure(out var error).ShouldBeTrue();
        error.Metadata.Count.ShouldBe(3);
        error.Metadata["Key"].ShouldBeNull();
        error.Metadata["key"].ShouldBe("value");
        error.Exception.ShouldBeSameAs(exception);
    }

    [Fact]
    public void Error_WithNullMetadataArray_ShouldRejectArray()
    {
        // Arrange
        KeyValuePair<string, object?>[] metadata = null!;

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => new Error("Failure", metadata));
    }

    [Fact]
    public void CollectionExpressionCalls_ShouldPreserveExistingResultAndErrorBehavior()
    {
        // Arrange
        var firstError = new Error("First error");
        var secondError = new Error("Second error");

        // Act
        var result = Result.Failure([firstError, secondError]);
        var valueResult = Result.Failure<int>([firstError, secondError]);
        var error = new Error("Failure", [new("Key", "Value")]);
        var emptyError = new Error("Failure", []);

        // Assert
        result.Errors.ShouldBe([firstError, secondError]);
        valueResult.Errors.ShouldBe([firstError, secondError]);
        error.Metadata["Key"].ShouldBe("Value");
        emptyError.Metadata.ShouldBeEmpty();
    }
}
