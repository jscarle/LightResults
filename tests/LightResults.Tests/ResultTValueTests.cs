using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace LightResults.Tests;

public class ResultTValueTests
{
    [Fact]
    public void Value_WhenResultIsSuccess_ShouldReturnAssignedValue()
    {
        // Arrange
        var result = Result<int>.Ok(42);

        // Act & Assert
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Value_WhenResultIsFailed_ShouldThrowException()
    {
        // Arrange
        var result = Result<int>.Fail("Error message");

        // Act & Assert
        result.Invoking(r => _ = r.Value).Should().Throw<InvalidOperationException>().WithMessage("Result is failed. Value is not set.");
    }

    [Fact]
    public void Ok_WithValue_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result<int>.Ok(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Should().BeEmpty();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Fail_ShouldCreateFailedResultWithSingleError()
    {
        // Act
        var result = Result<int>.Fail();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Message.Should().Be("");
    }

    [Fact]
    public void Fail_WithErrorMessage_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result<int>.Fail(errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Message.Should().Be(errorMessage);
    }

    [Fact]
    public void Fail_WithErrorMessageAndTupleMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        (string Key, object Value) metadata = ("Key", 0);

        // Act
        var result = Result<object>.Fail(errorMessage, metadata);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        var error = result.Errors.Should().ContainSingle().Which;
        error.Message.Should().Be(errorMessage);
        error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void Fail_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        IDictionary<string, object> metadata = new Dictionary<string, object> { { "Key", 0 } };

        // Act
        var result = Result<object>.Fail(errorMessage, metadata);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        var error = result.Errors.Should().ContainSingle().Which;
        error.Message.Should().Be(errorMessage);
        error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void Fail_WithErrorObject_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result<int>.Fail(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().BeEquivalentTo(error);
    }

    [Fact]
    public void Fail_WithErrorsEnumerable_ShouldCreateFailedResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2")
        };

        // Act
        var result = Result<int>.Fail(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().HaveCount(2).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void HasError_WithMatchingErrorType_ShouldReturnTrue()
    {
        // Arrange
        var result = Result<int>.Fail(new ValidationError("Validation error"));

        // Act & Assert
        result.HasError<ValidationError>().Should().BeTrue();
    }

    [Fact]
    public void HasError_WithNonMatchingErrorType_ShouldReturnFalse()
    {
        // Arrange
        var result = Result<int>.Fail(new Error("Generic error"));

        // Act & Assert
        result.HasError<ValidationError>().Should().BeFalse();
    }
    
    [Fact]
    public void Error_WhenResultIsFailed_ShouldReturnFirstError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError>
        {
            firstError,
            new Error("Error 2")
        };

        // Act
        var result = Result<int>.Fail(errors);

        // Assert
        result.Error.Should().Be(firstError);
    }

    [Fact]
    public void Error_WhenResultIsSuccess_ShouldThrowException()
    {
        // Arrange
        var result = Result<int>.Ok(42);

        // Act & Assert
        result.Invoking(r => _ = r.Error).Should().Throw<InvalidOperationException>().WithMessage("Result is successful. Error is not set.");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = True", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForBoolean(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<bool>.Ok(true) : Result<bool>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForSByte(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<sbyte>.Ok(1) : Result<sbyte>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForByte(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<byte>.Ok(1) : Result<byte>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForInt16(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<short>.Ok(1) : Result<short>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForUInt16(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<ushort>.Ok(1) : Result<ushort>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForInt32(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<int>.Ok(1) : Result<int>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForUInt32(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<uint>.Ok(1) : Result<uint>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForInt64(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<long>.Ok(1) : Result<long>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForUInt64(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<ulong>.Ok(1) : Result<ulong>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1.1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForDecimal(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<decimal>.Ok(1.1m) : Result<decimal>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1.1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForFloat(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<float>.Ok(1.1f) : Result<float>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1.1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForDouble(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<double>.Ok(1.1d) : Result<double>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 'c'", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForChar(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Ok('c') : Result<char>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"StringValue\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForString(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Ok("StringValue") : Result<string>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForObject(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Ok(new object()) : Result<object>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForNullableValueTypes(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<int?>.Ok(1) : Result<int?>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

#if NET7_0_OR_GREATER
    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForInt128(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<Int128>.Ok(1) : Result<Int128>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForUInt128(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result<UInt128>.Ok(1) : Result<UInt128>.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }
#endif
    
    
    [Fact]
    public void ImplicitOperator_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        Result<int> result = value;

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess
                .Should()
                .BeTrue();
            result.Value.Should()
                .Be(value);
            result.IsFailed
                .Should()
                .BeFalse();
            result.Errors
                .Should()
                .BeEmpty();
        }
    }
    
    private class ValidationError(string errorMessage) : Error(errorMessage);
}
