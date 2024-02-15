namespace LightResults;

/// <summary>Defines a result.</summary>
public interface IResult
{
    /// <summary>Gets a value indicating whether the result was successful.</summary>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    bool IsSuccess { get; }

    /// <summary>Gets a value indicating whether the result failed.</summary>
    /// <returns><c>true</c> if the result failed; otherwise, <c>false</c>.</returns>
    bool IsFailed { get; }

    /// <summary>Gets a collection of errors associated with the result.</summary>
    /// <returns>An <see cref="IReadOnlyCollection{T}" /> of <see cref="IError" /> representing the errors.</returns>
    IReadOnlyList<IError> Errors { get; }

    /// <summary>Checks if the result contains an error of the specific type.</summary>
    /// <typeparam name="TError">The type of error to check for.</typeparam>
    /// <returns><c>true</c> if an error of the specified type is present; otherwise, <c>false</c>.</returns>
    bool HasError<TError>() where TError : IError;
}

/// <summary>Defines a result with a value.</summary>
public interface IResult<out TValue> : IResult
{
    /// <summary>Gets the value of the result, throwing an exception if the result is failed.</summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to get or set the value of a failed result.</exception>
    public TValue Value { get; }
}
