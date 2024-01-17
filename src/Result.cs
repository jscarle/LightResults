using LightResults.Common;

namespace LightResults;

/// <summary>
/// Represents a result.
/// </summary>
public sealed class Result : ResultBase
#if NET8_0_OR_GREATER
    , IResult<Result>
#endif
{
    private Result()
    {
    }

    private Result(string errorMessage)
        : base(errorMessage)
    {
    }

    private Result(IError error)
        : base(error)
    {
    }

    private Result(IEnumerable<IError> errors)
        : base(errors)
    {
    }

    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <returns></returns>
    public static Result Ok()
    {
        return new Result();
    }

    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to include in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a success result with the specified value.</returns>
    public static Result<TValue> Ok<TValue>()
    {
        return Result<TValue>.Ok();
    }

    /// <summary>
    /// Creates a success result with the given value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to include in the result.</typeparam>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a success result with the specified value.</returns>
    public static Result<TValue> Ok<TValue>(TValue value)
    {
        return Result<TValue>.Ok(value);
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message.</returns>
    public static Result Fail()
    {
        return new Result(new Error());
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail<TValue>()
    {
        return Result<TValue>.Fail();
    }

    /// <summary>
    /// Creates a failed result with the given error message.
    /// </summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message.</returns>
    public static Result Fail(string errorMessage)
    {
        return new Result(errorMessage);
    }

    /// <summary>
    /// Creates a failed result with the given error message.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail<TValue>(string errorMessage)
    {
        return Result<TValue>.Fail(errorMessage);
    }

    /// <summary>
    /// Creates a failed result with the given error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error.</returns>
    public static Result Fail(IError error)
    {
        return new Result(error);
    }

    /// <summary>
    /// Creates a failed result with the given error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error.</returns>
    public static Result<TValue> Fail<TValue>(IError error)
    {
        return Result<TValue>.Fail(error);
    }

    /// <summary>
    /// Creates a failed result with the given errors.
    /// </summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified errors.</returns>
    public static Result Fail(IEnumerable<IError> errors)
    {
        return new Result(errors);
    }

    /// <summary>
    /// Creates a failed result with the given errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified errors.</returns>
    public static Result<TValue> Fail<TValue>(IEnumerable<IError> errors)
    {
        return Result<TValue>.Fail(errors);
    }
}

/// <summary>
/// Represents a result.
/// </summary>
/// <typeparam name="TValue">The type of the value in the result.</typeparam>
public sealed class Result<TValue> : ResultBase
#if NET8_0_OR_GREATER
    , IResult<Result<TValue>>
#endif
{
    /// <summary>
    /// Gets the value of the result, throwing an exception if the result is failed.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when attempting to get or set the value of a failed result.
    /// </exception>
    public TValue Value
    {
        get
        {
            if (IsFailed)
                throw new InvalidOperationException("Result is failed. Value is not set.");

            return _valueOrDefault!;
        }
        private set => _valueOrDefault = value;
    }

    private TValue? _valueOrDefault;

    private Result()
    {
    }

    private Result(string errorMessage)
        : base(errorMessage)
    {
    }

    private Result(IError error)
        : base(error)
    {
    }

    private Result(IEnumerable<IError> errors)
        : base(errors)
    {
    }

    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a success result with the specified value.</returns>
    public static Result<TValue> Ok()
    {
        return new Result<TValue>();
    }

    /// <summary>
    /// Creates a success result with the given value.
    /// </summary>
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

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail()
    {
        return new Result<TValue>(new Error());
    }

    /// <summary>
    /// Creates a failed result with the given error message.
    /// </summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail(string errorMessage)
    {
        return new Result<TValue>(errorMessage);
    }

    /// <summary>
    /// Creates a failed result with the given error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error.</returns>
    public static Result<TValue> Fail(IError error)
    {
        return new Result<TValue>(error);
    }

    /// <summary>
    /// Creates a failed result with the given errors.
    /// </summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified errors.</returns>
    public static Result<TValue> Fail(IEnumerable<IError> errors)
    {
        return new Result<TValue>(errors);
    }

    /// <summary>
    /// Implicitly converts a non-generic <see cref="Result" /> to a generic <see cref="Result{TValue}" />.
    /// </summary>
    /// <param name="result">The non-generic result to convert.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> with the specified value and errors from the non-generic result.</returns>
    public static implicit operator Result<TValue>(Result result)
    {
        return FromResult(result);
    }

    /// <summary>
    /// Converts a non-generic <see cref="Result" /> to a generic <see cref="Result{TValue}" />.
    /// </summary>
    /// <param name="result">The non-generic result to convert.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> with the specified value and errors from the non-generic result.</returns>
    public static Result<TValue> FromResult(Result result)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(result);
#else
        if (result is null)
            throw new ArgumentNullException(nameof(result));
#endif

        return new Result<TValue>(result.Errors);
    }

    /// <summary>
    /// Implicitly converts a value to a success result with the specified value.
    /// </summary>
    /// <param name="value">The value to convert to a success result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a success result with the specified value.</returns>
    public static implicit operator Result<TValue>(TValue value)
    {
        return FromValue(value);
    }

    /// <summary>
    /// Converts a value to a success result with the specified value.
    /// </summary>
    /// <param name="value">The value to convert to a success result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a success result with the specified value.</returns>
    public static Result<TValue> FromValue(TValue value)
    {
        return Ok(value);
    }
}
