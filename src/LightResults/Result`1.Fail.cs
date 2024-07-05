#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

namespace LightResults;

partial struct Result<TValue>
{
    internal static readonly Result<TValue> FailedResult = new(Error.Empty);

#if NET7_0_OR_GREATER
    /// <summary>Creates a failed result.</summary>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail()
    {
        return FailedResult;
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(string errorMessage)
    {
        var error = new Error(errorMessage);
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(string errorMessage, IReadOnlyDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(IError error)
    {
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified errors.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(IEnumerable<IError> errors)
    {
        return new Result<TValue>(errors);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified errors.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(IReadOnlyList<IError> errors)
    {
        return new Result<TValue>(errors);
    }
#endif
}
