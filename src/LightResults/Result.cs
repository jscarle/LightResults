using LightResults.Common;

namespace LightResults;

/// <summary>Represents a result.</summary>
public sealed class Result : ResultBase
#if NET7_0_OR_GREATER
    , IActionableResult<Result>
#endif
{
    private static readonly Result OkResult = new();
    private static readonly Result FailedResult = new(Error.Empty);

    private Result()
    {
    }

    private Result(IError error) : base(error)
    {
    }

    private Result(IEnumerable<IError> errors) : base(errors)
    {
    }

    /// <summary>Creates a success result.</summary>
    /// <returns>A new instance of <see cref="Result" /> representing a success result with the specified value.</returns>
    public static Result Ok()
    {
        return OkResult;
    }

    /// <summary>Creates a success result with the specified value.</summary>
    /// <param name="value">The value to include in the result.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a success result with the specified value.</returns>
    public static Result<TValue> Ok<TValue>(TValue value)
    {
        return Result<TValue>.Ok(value);
    }

    /// <summary>Creates a failed result.</summary>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result.</returns>
    public static Result Fail()
    {
        return FailedResult;
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message.</returns>
    public static Result Fail(string errorMessage)
    {
        var error = new Error(errorMessage);
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message and metadata.</returns>
    public static Result Fail(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message and metadata.</returns>
    public static Result Fail(string errorMessage, IDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error.</returns>
    public static Result Fail(IError error)
    {
        return new Result(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified errors.</returns>
    public static Result Fail(IEnumerable<IError> errors)
    {
        return new Result(errors);
    }

    /// <summary>Creates a failed result.</summary>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result.</returns>
    public static Result<TValue> Fail<TValue>()
    {
        return Result<TValue>.Fail();
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail<TValue>(string errorMessage)
    {
        var error = new Error(errorMessage);
        return Result<TValue>.Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message and metadata.</returns>
    public static Result<TValue> Fail<TValue>(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Result<TValue>.Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message and metadata.</returns>
    public static Result<TValue> Fail<TValue>(string errorMessage, IDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Result<TValue>.Fail(error);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error.</returns>
    public static Result<TValue> Fail<TValue>(IError error)
    {
        return Result<TValue>.Fail(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified errors.</returns>
    public static Result<TValue> Fail<TValue>(IEnumerable<IError> errors)
    {
        return Result<TValue>.Fail(errors);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (IsSuccess)
            return $"{nameof(Result)} {{ IsSuccess = True }}";

        if (Errors[0].Message.Length == 0)
            return $"{nameof(Result)} {{ IsSuccess = False }}";

        var errorString = GetErrorString();
        return GetResultString(nameof(Result), "False", errorString);
    }
}

/// <summary>Represents a result.</summary>
/// <typeparam name="TValue">The type of the value in the result.</typeparam>
public sealed class Result<TValue> : ResultBase
#if NET7_0_OR_GREATER
    , IActionableResult<TValue, Result<TValue>>
#else
    , IResult<TValue>
#endif
{
    private static readonly Result<TValue> FailedResult = new(Error.Empty);

    /// <summary>Gets the value of the result, throwing an exception if the result is failed.</summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to get or set the value of a failed result.</exception>
    public TValue Value
    {
        get
        {
            if (IsFailed)
                throw new InvalidOperationException("Result is failed. Value is not set.");

            return _valueOrDefault!;
        }
        private
#if NET6_0_OR_GREATER
        init
#else
        set
#endif
            => _valueOrDefault = value;
    }

    private
#if NET6_0_OR_GREATER
        readonly
#endif
        TValue? _valueOrDefault;

    private Result()
    {
    }

    private Result(IError error) : base(error)
    {
    }

    private Result(IEnumerable<IError> errors) : base(errors)
    {
    }

    /// <summary>Creates a success result with the specified value.</summary>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a success result with the specified value.</returns>
    public static Result<TValue> Ok(TValue value)
    {
        var result = new Result<TValue>
        {
            Value = value
        };
        return result;
    }

    /// <summary>Creates a failed result.</summary>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result.</returns>
    public static Result<TValue> Fail()
    {
        return FailedResult;
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail(string errorMessage)
    {
        var error = new Error(errorMessage);
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail(string errorMessage, IDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error.</returns>
    public static Result<TValue> Fail(IError error)
    {
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified errors.</returns>
    public static Result<TValue> Fail(IEnumerable<IError> errors)
    {
        return new Result<TValue>(errors);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (IsSuccess)
        {
            var valueString = GetValueString();
            return GetResultString(nameof(Result), "True", valueString);
        }

        if (Errors[0].Message.Length == 0)
            return $"{nameof(Result)} {{ IsSuccess = False }}";

        var errorString = GetErrorString();
        return GetResultString(nameof(Result), "False", errorString);
    }

    private string GetValueString()
    {
        if (IsFailed)
            return "";

        var valueString = Value?.ToString() ?? "";

        const string preValueStr = ", Value = ";
        const string charStr = "'";
        const string stringStr = "\"";

        if (Value is bool || Value is sbyte || Value is byte || Value is short || Value is ushort || Value is int || Value is uint || Value is long || Value is ulong ||
#if NET7_0_OR_GREATER
            Value is Int128 || Value is UInt128 ||
#endif
            Value is decimal || Value is float || Value is double)
        {
#if NET6_0_OR_GREATER
            var stringLength = preValueStr.Length + valueString.Length;

            var str = string.Create(stringLength, valueString, (span, state) => { span.TryWrite($"{preValueStr}{state}", out _); });

            return str;
#else
            return $"{preValueStr}{valueString}";
#endif
        }

        if (Value is char)
        {
#if NET6_0_OR_GREATER
            var stringLength = preValueStr.Length + charStr.Length + valueString.Length + charStr.Length;

            var str = string.Create(stringLength, valueString, (span, state) => { span.TryWrite($"{preValueStr}{charStr}{state}{charStr}", out _); });

            return str;
#else
            return $"{preValueStr}{charStr}{valueString}{charStr}";
#endif
        }

        if (Value is string)
        {
#if NET6_0_OR_GREATER
            var stringLength = preValueStr.Length + stringStr.Length + valueString.Length + stringStr.Length;

            var str = string.Create(stringLength, valueString, (span, state) => { span.TryWrite($"{preValueStr}{stringStr}{state}{stringStr}", out _); });

            return str;
#else
            return $"{preValueStr}{stringStr}{valueString}{stringStr}";
#endif
        }

        return "";
    }
}