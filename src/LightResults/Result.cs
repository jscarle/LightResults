using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using LightResults.Common;

namespace LightResults;

/// <summary>Represents a result.</summary>
public readonly struct Result :
#if NET7_0_OR_GREATER
    IActionableResult<Result>, IEquatable<Result>
#else
    IResult, IEquatable<Result>
#endif
{
    /// <inheritdoc/>
    public IReadOnlyCollection<IError> Errors => _errors ?? (_isSuccess ? Error.EmptyCollection : Error.DefaultCollection);

    private static readonly Result OkResult = new(true);
    private static readonly Result FailedResult = new(Error.Empty);
    private readonly bool _isSuccess = false;
    private readonly ImmutableArray<IError>? _errors;

    /// <summary>Initializes a new instance of the <see cref="Result"/> struct.</summary>
    public Result()
    {
    }

    private Result(bool isSuccess)
    {
        _isSuccess = isSuccess;
    }

    private Result(IError error)
    {
        _errors = ImmutableArray.Create(error);
    }

    private Result(IEnumerable<IError> errors)
    {
        _errors = errors.ToImmutableArray();
    }

    /// <inheritdoc/>
    public bool IsSuccess()
    {
        return _isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailed()
    {
        return !_isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailed([MaybeNullWhen(false)] out IError error)
    {
        error = _isSuccess ? default : GetError();
        return !_isSuccess;
    }

    /// <summary>Creates a success result.</summary>
    /// <returns>A new instance of <see cref="Result"/> representing a success result with the specified value.</returns>
    public static Result Ok()
    {
        return OkResult;
    }

    /// <summary>Creates a success result with the specified value.</summary>
    /// <param name="value">The value to include in the result.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a success result with the specified value.</returns>
    public static Result<TValue> Ok<TValue>(TValue value)
    {
        return Result<TValue>.Ok(value);
    }

    /// <summary>Creates a failed result.</summary>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result.</returns>
    public static Result Fail()
    {
        return FailedResult;
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error message.</returns>
    public static Result Fail(string errorMessage)
    {
        var error = new Error(errorMessage);
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error message and metadata.</returns>
    public static Result Fail(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error message and metadata.</returns>
    public static Result Fail(string errorMessage, IDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error.</returns>
    public static Result Fail(IError error)
    {
        return new Result(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified errors.</returns>
    public static Result Fail(IEnumerable<IError> errors)
    {
        return new Result(errors);
    }

    /// <summary>Creates a failed result.</summary>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result.</returns>
    public static Result<TValue> Fail<TValue>()
    {
        return Result<TValue>.Fail();
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Fail<TValue>(string errorMessage)
    {
        var error = new Error(errorMessage);
        return Result<TValue>.Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message and metadata.</returns>
    public static Result<TValue> Fail<TValue>(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Result<TValue>.Fail(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message and metadata.</returns>
    public static Result<TValue> Fail<TValue>(string errorMessage, IDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Result<TValue>.Fail(error);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error.</returns>
    public static Result<TValue> Fail<TValue>(IError error)
    {
        return Result<TValue>.Fail(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified errors.</returns>
    public static Result<TValue> Fail<TValue>(IEnumerable<IError> errors)
    {
        return Result<TValue>.Fail(errors);
    }

    /// <inheritdoc/>
    public bool HasError<TError>()
        where TError : IError
    {
        if (_isSuccess)
            return false;

        if (_errors is null && typeof(TError) == typeof(Error))
            return true;

        if (_errors is null || _errors.Value.Length == 0)
            return false;

        // Do not convert to LINQ, this creates unnecessary heap allocations.
        // For is the most efficient way to loop. It is the fastest and does not allocate.
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < _errors.Value.Length; index++)
        {
            var error = _errors.Value[index];
            if (error is TError)
                return true;
        }

        return false;
    }

    /// <summary>Matches the result and executes an action based on whether the result is successful or failed.</summary>
    /// <param name="success">A action to execute if the result is successful.</param>
    /// <param name="failure">A action to execute if the result is failed.</param>
    public void Match(Action success, Action<IError> failure)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(failure);
#else
        if (success is null)
            throw new ArgumentNullException(nameof(success));
        if (failure is null)
            throw new ArgumentNullException(nameof(failure));
#endif
        if (_isSuccess)
            success();
        else
            failure(GetError());
    }

    /// <summary>Matches the result and executes a function based on whether the result is successful or failed.</summary>
    /// <typeparam name="TResult">The type of the value to return.</typeparam>
    /// <param name="success">A function to execute if the result is successful.</param>
    /// <param name="failure">A function to execute if the result is failed.</param>
    /// <returns>The value of the executed function.</returns>
    public TResult Match<TResult>(Func<TResult> success, Func<IError, TResult> failure)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(failure);
#else
        if (success is null)
            throw new ArgumentNullException(nameof(success));
        if (failure is null)
            throw new ArgumentNullException(nameof(failure));
#endif
        return _isSuccess ? success() : failure(GetError());
    }

    /// <summary>Determines whether two <see cref="Result"/> instances are equal.</summary>
    /// <param name="other">The <see cref="Result"/> instance to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Result"/> is equal to this instance; otherwise, <c>false</c>.</returns>
    public bool Equals(Result other)
    {
        return Nullable.Equals(_errors, other._errors);
    }

    /// <summary>Determines whether the specified object is equal to this instance.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Result other && Equals(other);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return _errors.GetHashCode();
    }

    /// <summary>Determines whether two <see cref="Result"/> instances are equal.</summary>
    /// <param name="left">The first <see cref="Result"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Result"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Result"/> instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Result left, Result right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="Result"/> instances are not equal.</summary>
    /// <param name="left">The first <see cref="Result"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Result"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Result"/> instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Result left, Result right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_isSuccess)
            return $"{nameof(Result)} {{ IsSuccess = True }}";

        if (_errors is null || _errors.Value.Length == 0 || _errors.Value[0].Message.Length == 0)
            return $"{nameof(Result)} {{ IsSuccess = False }}";

        var errorString = StringHelper.GetResultErrorString(_errors.Value);
        return StringHelper.GetResultString(nameof(Result), "False", errorString);
    }

    private IError GetError()
    {
        return _errors is { Length: > 0 } ? _errors.Value[0] : Error.Empty;
    }
}
