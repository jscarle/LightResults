using FluentAssertions;
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

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = True")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForBoolean(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<bool>.Ok(true) : Result<bool>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForSByte(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<sbyte>.Ok(0) : Result<sbyte>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForByte(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<byte>.Ok(0) : Result<byte>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForInt16(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<short>.Ok(0) : Result<short>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForUInt16(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<ushort>.Ok(0) : Result<ushort>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForInt32(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<int>.Ok(0) : Result<int>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForUInt32(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<uint>.Ok(0) : Result<uint>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForInt64(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<long>.Ok(0) : Result<long>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForUInt64(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<ulong>.Ok(0) : Result<ulong>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForDecimal(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<decimal>.Ok(0) : Result<decimal>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForFloat(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<float>.Ok(0) : Result<float>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForDouble(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<double>.Ok(0) : Result<double>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 'c'")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForChar(bool success, string expected)
    {
        // Arrange
        var result = success ? Result.Ok('c') : Result<char>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"StringValue\"")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForString(bool success, string expected)
    {
        // Arrange
        var result = success ? Result.Ok("StringValue") : Result<string>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForObject(bool success, string expected)
    {
        // Arrange
        var result = success ? Result.Ok(new object()) : Result<object>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForNullableValueTypes(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<int?>.Ok(0) : Result<int?>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

#if NET7_0_OR_GREATER
    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForInt128(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<Int128>.Ok(0) : Result<Int128>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 0")]
    [InlineData(false, "IsSuccess = False")]
    public void ToString_ShouldReturnProperRepresentationForUInt128(bool success, string expected)
    {
        // Arrange
        var result = success ? Result<UInt128>.Ok(0) : Result<UInt128>.Fail();

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }
#endif
}