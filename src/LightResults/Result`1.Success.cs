#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

namespace LightResults;

partial struct Result<TValue>
{
#if NET7_0_OR_GREATER
    /// <summary>Creates a success result with the specified value.</summary>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a success result with the specified value.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Success(TValue value)
    {
        return new Result<TValue>(value);
    }
#endif
}
