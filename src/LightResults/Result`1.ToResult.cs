namespace LightResults;

partial struct Result<TValue>
{
    /// <summary>Converts the current <see cref="Result{TValue}"/> to a non-generic <see cref="Result"/> containing the same errors, if any.</summary>
    /// <returns>A new instance of <see cref="Result"/> representing the current result's errors, if any, or a successful result otherwise.</returns>
    /// <remarks>This method is useful for scenarios where a generic result needs to be converted into a non-generic result.</remarks>
    public Result ToResult()
    {
        return Result.Fail(Errors);
    }
}
