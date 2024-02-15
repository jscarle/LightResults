using System.Text;
using LightResults.Common;

namespace LightResults;

/// <summary>Represents a result.</summary>
public sealed class Result : ResultBase
#if NET7_0_OR_GREATER
    , IActionableResult<Result>
#endif
{
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
        return new Result();
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
        return new Result(new Error());
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message.</returns>
    public static Result Fail(string errorMessage)
    {
        return new Result(new Error(errorMessage));
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message and metadata.</returns>
    public static Result Fail(string errorMessage, (string Key, object Value) metadata)
    {
        return new Result(new Error(errorMessage, metadata));
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message and metadata.</returns>
    public static Result Fail(string errorMessage, IDictionary<string, object> metadata)
    {
        return new Result(new Error(errorMessage, metadata));
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
        return Result<TValue>.Fail(errorMessage);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message and metadata.</returns>
    public static Result<TValue> Fail<TValue>(string errorMessage, (string Key, object Value) metadata)
    {
        return Result<TValue>.Fail(errorMessage, metadata);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message and metadata.</returns>
    public static Result<TValue> Fail<TValue>(string errorMessage, IDictionary<string, object> metadata)
    {
        return Result<TValue>.Fail(errorMessage, metadata);
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
        return new Result<TValue>(new Error());
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail(string errorMessage)
    {
        return new Result<TValue>(new Error(errorMessage));
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail(string errorMessage, (string Key, object Value) metadata)
    {
        return new Result<TValue>(new Error(errorMessage, metadata));
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}" /> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail(string errorMessage, IDictionary<string, object> metadata)
    {
        return new Result<TValue>(new Error(errorMessage, metadata));
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
        var builder = new StringBuilder();
        builder.Append(nameof(Result));
        builder.Append(" { ");
        builder.Append("IsSuccess = ");
        builder.Append(IsSuccess);

        if (IsSuccess)
        {
            if (Value is bool || Value is sbyte || Value is byte || Value is short || Value is ushort || Value is int || Value is uint || Value is long || Value is ulong ||
#if NET7_0_OR_GREATER
                Value is Int128 || Value is UInt128 ||
#endif
                Value is decimal || Value is float || Value is double)
            {
                builder.Append(", Value = ");
                builder.Append(Value);
            }

            if (Value is char)
            {
                builder.Append(", Value = ");
                builder.Append('\'');
                builder.Append(Value);
                builder.Append('\'');
            }

            if (Value is string)
            {
                builder.Append(", Value = ");
                builder.Append('"');
                builder.Append(Value);
                builder.Append('"');
            }
        }

        if (IsFailed && Errors[0].Message.Length > 0)
        {
            builder.Append(", Error = ");
            builder.Append('"');
            builder.Append(Errors[0].Message);
            builder.Append('"');
        }

        builder.Append(" }");
        return builder.ToString();
    }
}
