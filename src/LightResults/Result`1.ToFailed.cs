namespace LightResults;

partial struct Result<TValue>
{
    /// <summary>Converts the current <see cref="Result{TValue}"/> to a failed <see cref="Result"/>.</summary>
    /// <returns>A new instance of <see cref="Result"/> containing the same error as the <see cref="Result{TValue}"/>, if any.</returns>
    public Result ToFailed()
    {
        if (ErrorsInternal is not null)
            return new Result(ErrorsInternal);

        return Result.FailedResult;
    }

    /// <summary>Converts the current <see cref="Result{TValue}"/> to a failed <see cref="Result{TDestination}"/>.</summary>
    /// <returns>A new instance of <see cref="Result{TDestination}"/> containing the same error as the <see cref="Result{TValue}"/>, if any.</returns>
    /// <typeparam name="TDestination">The type of the value of the failed result.</typeparam>
    public Result<TDestination> ToFailed<TDestination>()
    {
        if (ErrorsInternal is not null)
            return new Result<TDestination>(ErrorsInternal);

        return Result<TDestination>.FailedResult;
    }
}
