using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using LightResults.Common;

namespace LightResults;

// ReSharper disable StaticMemberInGenericType
/// <summary>Represents a result.</summary>
/// <typeparam name="TValue">The type of the value in the result.</typeparam>
public sealed class Result<TValue> :
#if NET7_0_OR_GREATER
    IActionableResult<TValue, Result<TValue>>
#else
    IResult<TValue>
#endif
{
    /// <inheritdoc />
    public bool IsSuccess => _errors.Length == 0;

    /// <inheritdoc />
    public bool IsFailed => _errors.Length != 0;

    /// <inheritdoc />
    public IReadOnlyCollection<IError> Errors => _errors;

    /// <inheritdoc />
    public IError Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException($"{nameof(Result)} is successful. {nameof(Error)} is not set.");

            return _errors[0];
        }
    }

    /// <inheritdoc />
    public TValue Value
    {
        get
        {
            if (IsFailed)
                throw new InvalidOperationException($"{nameof(Result)} is failed. {nameof(Value)} is not set.");

            return _valueOrDefault!;
        }
        private init => _valueOrDefault = value;
    }

    private static readonly Result<TValue> FailedResult = new(LightResults.Error.Empty);
    private readonly ImmutableArray<IError> _errors;
    private readonly TValue? _valueOrDefault;

    private Result()
    {
        _errors = ImmutableArray<IError>.Empty;
    }

    private Result(IError error)
    {
        _errors = ImmutableArray.Create(error);
    }

    private Result(IEnumerable<IError> errors)
    {
        _errors = errors.ToImmutableArray();
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
    public bool HasError<TError>() where TError : IError
    {
        // Do not convert to LINQ, this creates unnecessary heap allocations.
        // For is the most efficient way to loop. It is the fastest and does not allocate.
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < _errors.Length; index++)
        {
            var error = _errors[index];
            if (error is TError)
                return true;
        }

        return false;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (IsSuccess)
        {
            var valueString = StringHelper.GetResultValueString(_valueOrDefault);
            return StringHelper.GetResultString(nameof(Result), "True", valueString);
        }

        if (_errors[0].Message.Length == 0)
            return $"{nameof(Result)} {{ IsSuccess = False }}";

        var errorString = StringHelper.GetResultErrorString(_errors);
        return StringHelper.GetResultString(nameof(Result), "False", errorString);
    }
    
    /// <summary>Implicitly converts a value to a success <see cref="Result{TValue}"/>.</summary>
    /// <param name="value">The value to convert into a success result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a success result with the specified value.</returns>
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = $"{nameof(Ok)} is the named alternate.")]
    public static implicit operator Result<TValue>(TValue value)
    {
        return Ok(value);
    }
}