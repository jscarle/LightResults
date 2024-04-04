using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

// ReSharper disable SuspiciousTypeConversion.Global

namespace LightResults.Tests;

public sealed class ResultTValueTests
{
    [Fact]
    public void DefaultStruct_ShouldBeFailedResultWithDefaultValue()
    {
        // Arrange
        Result<int> result = default;

        // Act & Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out var resultValue).Should().BeFalse();
            resultValue.Should().Be(default);
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out var resultError).Should().BeTrue();
            resultError.Should().Be(Error.Empty);
            result.Errors.Should().ContainSingle().Which.Should().BeOfType<Error>();
            result.HasError<Error>().Should().BeTrue();
            result.HasError<ValidationError>().Should().BeFalse();
        }
    }

    [Fact]
    public void DefaultStruct_ShouldBeFailedResultWithNullValue()
    {
        // Arrange
        Result<object> result = default;

        // Act & Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out var resultValue).Should().BeFalse();
            resultValue.Should().Be(null);
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out var resultError).Should().BeTrue();
            resultError.Should().Be(Error.Empty);
            result.Errors.Should().ContainSingle().Which.Should().BeOfType<Error>();
            result.HasError<Error>().Should().BeTrue();
            result.HasError<ValidationError>().Should().BeFalse();
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act & Assert
        result.IsSuccess().Should().BeTrue();
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess_ShouldReturnAssignedValue()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should().BeTrue();
            resultValue.Should().Be(42);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess_ShouldReturnAssignedValueAndNullError()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue, out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should().BeTrue();
            resultValue.Should().Be(42);
            resultError.Should().Be(null);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailed_ShouldReturnDefaultValue()
    {
        // Arrange
        var result = Result.Fail<int>("Error message");

        // Act
        var isSuccess = result.IsSuccess(out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should().BeFalse();
            resultValue.Should().Be(default);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailed_ShouldReturnNullValue()
    {
        // Arrange
        var result = Result.Fail<object>("Error message");

        // Act
        var isSuccess = result.IsSuccess(out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should().BeFalse();
            resultValue.Should().Be(null);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailed_ShouldReturnDefaultValueAndFirstError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError> { firstError, new Error("Error 2") };
        var result = Result.Fail<int>(errors);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue, out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should().BeFalse();
            resultValue.Should().Be(default);
            resultError.Should().Be(firstError);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailed_ShouldReturnNullValueAndFirstError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError> { firstError, new Error("Error 2") };
        var result = Result.Fail<object>(errors);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue, out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should().BeFalse();
            resultValue.Should().Be(null);
            resultError.Should().Be(firstError);
        }
    }

    [Fact]
    public void IsFailed_WhenResultIsFailed()
    {
        // Arrange
        var result = Result.Fail<int>();

        // Act & Assert
        result.IsFailed().Should().BeTrue();
    }

    [Fact]
    public void IsFailed_WhenResultIsFailed_ShouldReturnFirstError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError> { firstError, new Error("Error 2") };
        var result = Result.Fail<int>(errors);

        // Act
        var isFailed = result.IsFailed(out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isFailed.Should().BeTrue();
            resultError.Should().Be(firstError);
        }
    }

    [Fact]
    public void IsFailed_WhenResultIsSuccess_ShouldReturnNullError()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act
        var isFailed = result.IsFailed(out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isFailed.Should().BeFalse();
            resultError.Should().Be(null);
        }
    }

    [Fact]
    public void IsFailed_WhenResultIsFailed_ShouldReturnFirstErrorAndDefaultValue()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError> { firstError, new Error("Error 2") };
        var result = Result.Fail<int>(errors);

        // Act
        var isFailed = result.IsFailed(out var resultError, out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isFailed.Should().BeTrue();
            resultError.Should().Be(firstError);
            resultValue.Should().Be(default);
        }
    }

    [Fact]
    public void IsFailed_WhenResultIsFailed_ShouldReturnFirstErrorAndNullValue()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError> { firstError, new Error("Error 2") };
        var result = Result.Fail<object>(errors);

        // Act
        var isFailed = result.IsFailed(out var resultError, out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isFailed.Should().BeTrue();
            resultError.Should().Be(firstError);
            resultValue.Should().Be(null);
        }
    }

    [Fact]
    public void IsFailed_WhenResultIsSuccess_ShouldReturnNullErrorAndAssignedValue()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act
        var isFailed = result.IsFailed(out var resultError, out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isFailed.Should().BeFalse();
            resultError.Should().Be(null);
            resultValue.Should().Be(42);
        }
    }

    [Fact]
    public void Ok_WithValue_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result.Ok(value);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeTrue();
            result.IsSuccess(out var resultValue).Should().BeTrue();
            resultValue.Should().Be(value);
            result.IsFailed().Should().BeFalse();
            result.IsFailed(out var resultError).Should().BeFalse();
            resultError.Should().Be(null);
            result.Errors.Should().BeEmpty();
        }
    }

    [Fact]
    public void Fail_ShouldCreateFailedResultWithSingleError()
    {
        // Act
        var result = Result.Fail<int>();

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("");
        }
    }

    [Fact]
    public void Fail_WithErrorMessage_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Fail<int>(errorMessage);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be(errorMessage);
        }
    }

    [Fact]
    public void Fail_WithErrorMessageAndTupleMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        (string Key, object Value) metadata = ("Key", 0);

        // Act
        var result = Result.Fail<object>(errorMessage, metadata);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            var error = result.Errors.Should().ContainSingle().Which;
            error.Message.Should().Be(errorMessage);
            error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void Fail_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        IDictionary<string, object> metadata = new Dictionary<string, object> { { "Key", 0 } };

        // Act
        var result = Result.Fail<object>(errorMessage, metadata);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            var error = result.Errors.Should().ContainSingle().Which;
            error.Message.Should().Be(errorMessage);
            error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void Fail_WithErrorObject_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Fail<int>(error);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Should().BeEquivalentTo(error);
        }
    }

    [Fact]
    public void Fail_WithErrorsEnumerable_ShouldCreateFailedResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError> { new Error("Error 1"), new Error("Error 2") };

        // Act
        var result = Result.Fail<int>(errors);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().HaveCount(2).And.BeEquivalentTo(errors);
        }
    }

    [Fact]
    public void HasError_WithMatchingErrorType_ShouldReturnTrue()
    {
        // Arrange
        var result = Result.Fail<int>(new ValidationError("Validation error"));

        // Act & Assert
        result.HasError<ValidationError>().Should().BeTrue();
    }

    [Fact]
    public void HasError_WithNonMatchingErrorType_ShouldReturnFalse()
    {
        // Arrange
        var result = Result.Fail<int>(new Error("Generic error"));

        // Act & Assert
        result.HasError<ValidationError>().Should().BeFalse();
    }

    [Fact]
    public void HasError_WhenIsSuccess_ShouldReturnFalse()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act & Assert
        result.HasError<ValidationError>().Should().BeFalse();
    }

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
            result.IsSuccess().Should().BeTrue();
            result.IsSuccess(out var resultValue).Should().BeTrue();
            resultValue.Should().Be(value);
            result.IsFailed().Should().BeFalse();
            result.IsFailed(out _).Should().BeFalse();
            result.Errors.Should().BeEmpty();
        }
    }

    [Fact]
    public void ToResult_ShouldConvertResultToNonGenericResultWithSameErrors()
    {
        // Arrange
        var errors = new List<IError> { new Error("Error 1"), new Error("Error 2") };
        var result = Result.Fail<int>(errors);

        // Act
        var nonGenericResult = result.ToResult();

        // Assert
        using (new AssertionScope())
        {
            nonGenericResult.IsSuccess().Should().BeFalse();
            nonGenericResult.IsFailed().Should().BeTrue();
            nonGenericResult.Errors.Should().HaveCount(2).And.BeEquivalentTo(errors);
        }
    }

    [Fact]
    public void Equals_ResultInt_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(42);

        // Act & Assert
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ResultInt_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(43);

        // Act & Assert
        result1.Equals(result2).Should().BeFalse();
    }

    [Fact]
    public void Equals_ResultObject_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok<object>("test");
        var result2 = Result.Ok<object>("test");

        // Act & Assert
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ResultObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok<object>("test1");
        var result2 = Result.Ok<object>("test2");

        // Act & Assert
        result1.Equals(result2).Should().BeFalse();
    }

    [Fact]
    public void Equals_ResultIntToObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok<object>(42);

        // Act & Assert
        result1.Equals(result2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ResultInt_ShouldReturnSameHashCodeForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(42);

        // Act & Assert
        result1.GetHashCode().Should().Be(result2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ResultInt_ShouldReturnDifferentHashCodeForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(43);

        // Act & Assert
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
    }

    [Fact]
    public void op_Equality_ResultInt_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(42);

        // Act & Assert
        (result1 == result2).Should().BeTrue();
    }

    [Fact]
    public void op_Equality_ResultInt_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(43);

        // Act & Assert
        (result1 == result2).Should().BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultInt_ShouldReturnFalseForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(42);

        // Act & Assert
        (result1 != result2).Should().BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultInt_ShouldReturnTrueForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(43);

        // Act & Assert
        (result1 != result2).Should().BeTrue();
    }

    [Fact]
    public void op_Equality_ResultObject_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok<object>("test");
        var result2 = Result.Ok<object>("test");

        // Act & Assert
        (result1 == result2).Should().BeTrue();
    }

    [Fact]
    public void op_Equality_ResultObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok<object>("test1");
        var result2 = Result.Ok<object>("test2");

        // Act & Assert
        (result1 == result2).Should().BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultObject_ShouldReturnFalseForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok<object>("test");
        var result2 = Result.Ok<object>("test");

        // Act & Assert
        (result1 != result2).Should().BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultObject_ShouldReturnTrueForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok<object>("test1");
        var result2 = Result.Ok<object>("test2");

        // Act & Assert
        (result1 != result2).Should().BeTrue();
    }

    [Fact]
    public void op_Equality_ResultIntToObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok<object>(42);

        // Act & Assert
        (result1 == result2).Should().BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultIntToObject_ShouldReturnTrueForDifferentResults()
    {
        // Arrange
        var result1 = Result.Ok(42);
        var result2 = Result.Ok<object>(42);

        // Act & Assert
        (result1 != result2).Should().BeTrue();
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = True", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForBoolean(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Ok(true) : Result.Fail<bool>(errorMessage);

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
        var result = success ? Result.Ok<sbyte>(1) : Result.Fail<sbyte>(errorMessage);

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
        var result = success ? Result.Ok<byte>(1) : Result.Fail<byte>(errorMessage);

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
        var result = success ? Result.Ok<short>(1) : Result.Fail<short>(errorMessage);

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
        var result = success ? Result.Ok<ushort>(1) : Result.Fail<ushort>(errorMessage);

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
        var result = success ? Result.Ok(1) : Result.Fail<int>(errorMessage);

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
        var result = success ? Result.Ok<uint>(1) : Result.Fail<uint>(errorMessage);

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
        var result = success ? Result.Ok<long>(1) : Result.Fail<long>(errorMessage);

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
        var result = success ? Result.Ok<ulong>(1) : Result.Fail<ulong>(errorMessage);

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
        var result = success ? Result.Ok(1.1m) : Result.Fail<decimal>(errorMessage);

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
        var result = success ? Result.Ok(1.1f) : Result.Fail<float>(errorMessage);

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
        var result = success ? Result.Ok(1.1d) : Result.Fail<double>(errorMessage);

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
        var result = success ? Result.Ok('c') : Result.Fail<char>(errorMessage);

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
        var result = success ? Result.Ok("StringValue") : Result.Fail<string>(errorMessage);

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
        var result = success ? Result.Ok(new object()) : Result.Fail<object>(errorMessage);

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
        var result = success ? Result.Ok<int?>(1) : Result.Fail<int?>(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }

    private class ValidationError(string errorMessage) : Error(errorMessage);

#if NET7_0_OR_GREATER
    [Fact]
    public void InterfaceOk_WithValue_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = (IActionableResult<int, Result<int>>)Result.Ok(value);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeTrue();
            result.IsSuccess(out var resultValue).Should().BeTrue();
            resultValue.Should().Be(value);
            result.IsFailed().Should().BeFalse();
            result.IsFailed(out var resultError).Should().BeFalse();
            resultError.Should().Be(null);
            result.Errors.Should().BeEmpty();
        }
    }

    [Fact]
    public void InterfaceFail_ShouldCreateFailedResultWithSingleError()
    {
        // Act
        var result = (IActionableResult<int, Result<int>>)Result.Fail<int>();

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("");
        }
    }

    [Fact]
    public void InterfaceFail_WithErrorMessage_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = (IActionableResult<int, Result<int>>)Result.Fail<int>(errorMessage);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be(errorMessage);
        }
    }

    [Fact]
    public void InterfaceFail_WithErrorMessageAndTupleMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        (string Key, object Value) metadata = ("Key", 0);

        // Act
        var result = (IActionableResult<object, Result<object>>)Result.Fail<object>(errorMessage, metadata);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            var error = result.Errors.Should().ContainSingle().Which;
            error.Message.Should().Be(errorMessage);
            error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void InterfaceFail_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        IDictionary<string, object> metadata = new Dictionary<string, object> { { "Key", 0 } };

        // Act
        var result = (IActionableResult<object, Result<object>>)Result.Fail<object>(errorMessage, metadata);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            var error = result.Errors.Should().ContainSingle().Which;
            error.Message.Should().Be(errorMessage);
            error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void InterfaceFail_WithErrorObject_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = (IActionableResult<int, Result<int>>)Result.Fail<int>(error);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Should().BeEquivalentTo(error);
        }
    }

    [Fact]
    public void InterfaceFail_WithErrorsEnumerable_ShouldCreateFailedResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2")
        };

        // Act
        var result = (IActionableResult<int, Result<int>>)Result.Fail<int>(errors);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().HaveCount(2).And.BeEquivalentTo(errors);
        }
    }
#endif

#if NET7_0_OR_GREATER
    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForInt128(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Ok<Int128>(1) : Result.Fail<Int128>(errorMessage);

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
        var result = success ? Result.Ok<UInt128>(1) : Result.Fail<UInt128>(errorMessage);

        // Act & Assert
        result.ToString().Should().Be($"Result {{ {expected} }}");
    }
#endif
}
