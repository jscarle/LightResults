#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

namespace LightResults;

partial struct Result<TValue>
{
    internal static Result<TValue> Ok(TValue value)
    {
        var result = new Result<TValue>(value);
        return result;
    }

#if NET7_0_OR_GREATER
    /// <summary>Creates a success result with the specified value.</summary>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a success result with the specified value.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Ok(TValue value)
    {
        return Ok(value);
    }
#endif
}
