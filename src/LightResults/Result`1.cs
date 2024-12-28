using System.Diagnostics.CodeAnalysis;
using LightResults.Common;

namespace LightResults;

// ReSharper disable StaticMemberInGenericType
/// <summary>Represents a result.</summary>
/// <typeparam name="TValue">The type of the value of the result.</typeparam>
public readonly struct Result<TValue> : IEquatable<Result<TValue>>,
#if NET7_0_OR_GREATER
    IActionableResult<TValue, Result<TValue>>
#else
    IResult<TValue>
#endif
{
    /// <inheritdoc/>
    public IReadOnlyList<IError> Errors
    {
        get
        {
            if (_errors is not null)
                return _errors;

            if (_isSuccess)
                return Error.EmptyErrorList;

            return Error.DefaultErrorList;
        }
    }

    internal static readonly Result<TValue> FailureResult = new(Error.Empty);
    private readonly bool _isSuccess = false;
    private readonly IReadOnlyList<IError>? _errors;
    private readonly TValue? _valueOrDefault;

    internal Result(TValue value)
    {
        _isSuccess = true;
        _valueOrDefault = value;
    }

    internal Result(IError error)
    {
        _errors = [error];
    }

    internal Result(IEnumerable<IError> errors)
    {
        _errors = errors.ToArray();
    }

    private Result(IReadOnlyList<IError> errors)
    {
        _errors = errors;
    }

    /// <inheritdoc/>
    public bool IsSuccess()
    {
        return _isSuccess;
    }

    /// <inheritdoc/>
    public bool IsSuccess([MaybeNullWhen(false)] out TValue value)
    {
        value = _valueOrDefault;
        return _isSuccess;
    }

    /// <inheritdoc/>
    public bool IsSuccess([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out IError error)
    {
        if (_isSuccess)
        {
            value = _valueOrDefault;
            error = default;
        }
        else
        {
            value = default;
            if (_errors is not null)
                error = _errors[0];
            else
                error = Error.Empty;
        }

        return _isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailure()
    {
        return !_isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailure([MaybeNullWhen(false)] out IError error)
    {
        if (_isSuccess)
            error = default;
        else if (_errors is not null)
            error = _errors[0];
        else
            error = Error.Empty;

        return !_isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailure([MaybeNullWhen(false)] out IError error, [MaybeNullWhen(true)] out TValue value)
    {
        if (_isSuccess)
        {
            value = _valueOrDefault;
            error = default;
        }
        else
        {
            value = default;
            if (_errors is not null)
                error = _errors[0];
            else
                error = Error.Empty;
        }

        return !_isSuccess;
    }

#if NET7_0_OR_GREATER
    /// <summary>Creates a success result with the specified value.</summary>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a success result with the specified value.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Success(TValue value)
    {
        return new Result<TValue>(value);
    }

    /// <summary>Creates a failed result.</summary>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Failure()
    {
        return FailureResult;
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Failure(string errorMessage)
    {
        var error = new Error(errorMessage);
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Failure(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Failure(string errorMessage, IReadOnlyDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Failure(IError error)
    {
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified errors.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Failure(IEnumerable<IError> errors)
    {
        return new Result<TValue>(errors);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified errors.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Failure(IReadOnlyList<IError> errors)
    {
        return new Result<TValue>(errors);
    }
#endif

    /// <inheritdoc/>
    public bool HasError<TError>()
        where TError : IError
    {
        if (_isSuccess)
            return false;

        if (_errors is not null)
            // Do not convert to LINQ, this creates unnecessary heap allocations.
            // For is the most efficient way to loop. It is the fastest and does not allocate.
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < _errors.Count; index++)
            {
                if (_errors[index] is TError)
                    return true;
            }

        return typeof(TError) == typeof(Error);
    }

    /// <inheritdoc/>
    public bool HasError<TError>([MaybeNullWhen(false)] out TError error)
        where TError : IError
    {
        if (_isSuccess)
        {
            error = default;
            return false;
        }

        if (_errors is not null)
            // Do not convert to LINQ, this creates unnecessary heap allocations.
            // For is the most efficient way to loop. It is the fastest and does not allocate.
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < _errors.Count; index++)
            {
                if (_errors[index] is not TError tError)
                    continue;

                error = tError;
                return true;
            }

        if (typeof(TError) == typeof(Error))
        {
            error = (TError)Error.Empty;
            return true;
        }

        error = default;
        return false;
    }

    /// <summary>Implicitly converts a value to a success <see cref="Result{TValue}"/>.</summary>
    /// <param name="value">The value to convert into a success result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a success result with the specified value.</returns>
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = "We don't want to expose named alternates in this case.")]
    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value);
    }

    /// <summary>Implicitly converts an <see cref="Error"/> to a failure <see cref="Result{TValue}"/>.</summary>
    /// <param name="error">The error to convert into a failure result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failure result with the specified error.</returns>
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = "We don't want to expose named alternates in this case.")]
    public static implicit operator Result<TValue>(Error error)
    {
        return new Result<TValue>(error);
    }

    /// <summary>Converts the current <see cref="Result{TValue}"/> to a failed <see cref="Result"/>.</summary>
    /// <returns>A new instance of <see cref="Result"/> containing the same error as the <see cref="Result{TValue}"/>, if any.</returns>
    public Result AsFailure()
    {
        if (_errors is not null)
            return new Result(_errors);

        return Result.FailureResult;
    }

    /// <summary>Converts the current <see cref="Result{TValue}"/> to a failed <see cref="Result{TDestination}"/>.</summary>
    /// <returns>A new instance of <see cref="Result{TDestination}"/> containing the same error as the <see cref="Result{TValue}"/>, if any.</returns>
    /// <typeparam name="TDestination">The type of the value of the failed result.</typeparam>
    public Result<TDestination> AsFailure<TDestination>()
    {
        if (_errors is not null)
            return new Result<TDestination>(_errors);

        return Result<TDestination>.FailureResult;
    }

    /// <summary>Determines whether two <see cref="Result{TValue}"/> instances are equal.</summary>
    /// <param name="other">The <see cref="Result{TValue}"/> instance to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Result{TValue}"/> is equal to this instance; otherwise, <c>false</c>.</returns>
    public bool Equals(Result<TValue> other)
    {
        return Equals(_errors, other._errors) && EqualityComparer<TValue?>.Default.Equals(_valueOrDefault, other._valueOrDefault);
    }

    /// <summary>Determines whether the specified object is equal to this instance.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Result<TValue> other && Equals(other);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(_errors, _valueOrDefault);
    }

    /// <summary>Determines whether two <see cref="Result{TValue}"/> instances are equal.</summary>
    /// <param name="left">The first <see cref="Result{TValue}"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Result{TValue}"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Result{TValue}"/> instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Result<TValue> left, Result<TValue> right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="Result{TValue}"/> instances are not equal.</summary>
    /// <param name="left">The first <see cref="Result{TValue}"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Result{TValue}"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Result{TValue}"/> instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Result<TValue> left, Result<TValue> right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_isSuccess)
            return StringHelper.GetResultValueString(_valueOrDefault);

        if (_errors is not null && _errors[0].Message.Length > 0)
            return StringHelper.GetResultErrorString(_errors[0].Message);

        return $"{nameof(Result)} {{ IsSuccess = False }}";
    }
}
