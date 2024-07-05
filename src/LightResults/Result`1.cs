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
    private readonly bool _isSuccess = false;
    private readonly IReadOnlyList<IError>? _errors;
    private readonly TValue? _valueOrDefault;

    internal Result(TValue value)
    {
        _isSuccess = true;
        _valueOrDefault = value;
    }

    internal Result(IError error)
    {
        _errors = [error];
    }

    internal Result(IEnumerable<IError> errors)
    {
        _errors = errors.ToArray();
    }

    private Result(IReadOnlyList<IError> errors)
    {
        _errors = errors;
    }
}
