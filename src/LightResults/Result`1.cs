#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

namespace LightResults;

// ReSharper disable StaticMemberInGenericType
/// <summary>Represents a result.</summary>
/// <typeparam name="TValue">The type of the value of the result.</typeparam>
public readonly partial struct Result<TValue> : IEquatable<Result<TValue>>,
#if NET7_0_OR_GREATER
    IActionableResult<TValue, Result<TValue>>
#else
    IResult<TValue>
#endif
{
    internal readonly bool IsSuccessInternal = false;
    internal readonly IReadOnlyList<IError>? ErrorsInternal;
    internal readonly TValue? ValueOrDefaultInternal;

    internal Result(TValue value)
    {
        IsSuccessInternal = true;
        ValueOrDefaultInternal = value;
    }

    internal Result(IError error)
    {
        ErrorsInternal = [error];
    }

    internal Result(IEnumerable<IError> errors)
    {
        ErrorsInternal = errors.ToArray();
    }

    private Result(IReadOnlyList<IError> errors)
    {
        ErrorsInternal = errors;
    }
}
