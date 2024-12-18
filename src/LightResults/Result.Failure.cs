namespace LightResults;

partial struct Result
{
    internal static readonly Result FailureResult = new(Error.Empty);

    /// <summary>Creates a failed result.</summary>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result.</returns>
    public static Result Failure()
    {
        return FailureResult;
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error message.</returns>
    public static Result Failure(string errorMessage)
    {
        var error = new Error(errorMessage);
        return new Result(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error message and metadata.</returns>
    public static Result Failure(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return new Result(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error message and metadata.</returns>
    public static Result Failure(string errorMessage, IReadOnlyDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return new Result(error);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error.</returns>
    public static Result Failure(IError error)
    {
        return new Result(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified errors.</returns>
    public static Result Failure(IEnumerable<IError> errors)
    {
        return new Result(errors);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified errors.</returns>
    public static Result Failure(IReadOnlyList<IError> errors)
    {
        return new Result(errors);
    }
}
