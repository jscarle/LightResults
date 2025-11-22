using Shouldly;

namespace LightResults.Tests;

public sealed class ImplicitOperatorTests
{
    [Fact]
    public void ImplicitErrorConversion_ToResult_ShouldBeFailureWithMetadata()
    {
        // Arrange
        var error = new Error("boom", new KeyValuePair<string, object?>("CorrelationId", "123"));

        // Act
        Result result = error;

        // Assert
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out var convertedError).ShouldBeTrue();
        convertedError.ShouldBe(error);

        var storedError = result.Errors.ShouldHaveSingleItem();
        storedError.Message.ShouldBe("boom");
        storedError.Metadata.ShouldContainKey("CorrelationId");
        storedError.Metadata["CorrelationId"].ShouldBe("123");
    }

    [Fact]
    public void ImplicitErrorConversion_ToResultOfT_ShouldBeFailureWithMetadata()
    {
        // Arrange
        var error = new Error("boom", new KeyValuePair<string, object?>("CorrelationId", "456"));

        // Act
        Result<int> result = error;

        // Assert
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out var convertedError).ShouldBeTrue();
        convertedError.ShouldBe(error);

        var storedError = result.Errors.ShouldHaveSingleItem();
        storedError.Message.ShouldBe("boom");
        storedError.Metadata.ShouldContainKey("CorrelationId");
        storedError.Metadata["CorrelationId"].ShouldBe("456");
    }

    [Fact]
    public void ImplicitValueConversion_ToResultOfT_ShouldSucceedForPrimitive()
    {
        // Act
        Result<int> result = 42;

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(42);
        result.IsFailure().ShouldBeFalse();
    }

    [Fact]
    public void ImplicitValueConversion_ToResultOfT_ShouldTreatDefaultValueTypeAsSuccess()
    {
        // Act
        Result<int> result = 0;

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(0);
        result.IsFailure().ShouldBeFalse();
    }

    [Fact]
    public void ImplicitValueConversion_ToResultOfT_ShouldHandleReferenceValues()
    {
        // Act
        Result<string> result = "hello";

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe("hello");
        result.IsFailure().ShouldBeFalse();
    }

    [Fact]
    public void ImplicitValueConversion_ToResultOfT_ShouldAllowNullReferences()
    {
        // Arrange
        string? nullValue = null;

        // Act
        Result<string?> result = nullValue;

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBeNull();
        result.IsFailure().ShouldBeFalse();
    }
}
