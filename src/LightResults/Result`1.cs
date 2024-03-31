using System.Collections.Immutable;
#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

namespace LightResults;

// ReSharper disable StaticMemberInGenericType
/// <summary>Represents a result.</summary>
/// <typeparam name="TValue">The type of the value in the result.</typeparam>
public readonly partial struct Result<TValue> : IEquatable<Result<TValue>>,
#if NET7_0_OR_GREATER
    IActionableResult<TValue, Result<TValue>>
#else
    IResult<TValue>
#endif
{
    private readonly bool _isSuccess = false;
    private readonly ImmutableArray<IError>? _errors;
    private readonly TValue? _valueOrDefault;

    /// <summary>Initializes a new instance of the <see cref="Result{TValue}"/> struct.</summary>
    public Result()
    {
    }

    private Result(TValue value)
    {
        _isSuccess = true;
        _valueOrDefault = value;
    }

    private Result(IError error)
    {
        _errors = ImmutableArray.Create(error);
    }

    private Result(IEnumerable<IError> errors)
    {
        _errors = errors.ToImmutableArray();
    }
}
