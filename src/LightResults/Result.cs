using System.Collections.Immutable;
#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

namespace LightResults;

/// <summary>Represents a result.</summary>
public readonly partial struct Result : IEquatable<Result>,
#if NET7_0_OR_GREATER
    IActionableResult<Result>
#else
    IResult
#endif
{
    private readonly bool _isSuccess = false;
    private readonly ImmutableArray<IError>? _errors;

    /// <summary>Initializes a new instance of the <see cref="Result"/> struct.</summary>
    public Result()
    {
    }

    private Result(bool isSuccess)
    {
        _isSuccess = isSuccess;
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
