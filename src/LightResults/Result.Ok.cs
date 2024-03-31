namespace LightResults;

partial struct Result
{
    private static readonly Result OkResult = new(true);

    /// <summary>Creates a success result.</summary>
    /// <returns>A new instance of <see cref="Result"/> representing a success result with the specified value.</returns>
    public static Result Ok()
    {
        return OkResult;
    }
}
