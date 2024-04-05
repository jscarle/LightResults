namespace LightResults;

partial struct Result<TValue>
{
    /// <summary>Converts the current <see cref="Result{TValue}"/> to a failed <see cref="Result"/>.</summary>
    /// <returns>A new instance of <see cref="Result"/> containing the same error as the <see cref="Result{TValue}"/>, if any.</returns>
    public Result AsFailed()
    {
        if (_errors.HasValue)
            return new Result(_errors.Value);

        return Result.FailedResult;
    }

    /// <summary>Converts the current <see cref="Result{TValue}"/> to a failed <see cref="Result{TDestination}"/>.</summary>
    /// <returns>A new instance of <see cref="Result{TDestination}"/> containing the same error as the <see cref="Result{TValue}"/>, if any.</returns>
    /// <typeparam name="TDestination">The type of the value of the failed result.</typeparam>
    public Result<TDestination> AsFailed<TDestination>()
    {
        if (_errors.HasValue)
            return new Result<TDestination>(_errors.Value);

        return Result<TDestination>.FailedResult;
    }
}
