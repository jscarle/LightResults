using System.Collections;
using System.Collections.Immutable;

namespace LightResults.Common;

internal sealed class ErrorArray : IReadOnlyList<IError>
{
    public int Count => _errors.Length;

    public IError this[int index] => _errors[index];
    public static readonly ErrorArray Empty = new();

    private readonly ImmutableArray<IError> _errors;

    public ErrorArray(IError error)
    {
        _errors = ImmutableArray.Create(error);
    }

    public ErrorArray(IEnumerable<IError> errors)
    {
        _errors = errors.ToImmutableArray();
    }

    private ErrorArray()
    {
        _errors = ImmutableArray<IError>.Empty;
    }

    public IEnumerator<IError> GetEnumerator()
    {
        // Do not convert to LINQ, this creates unnecessary heap allocations.
        // For is the most efficient way to loop. It is the fastest and does not allocate.
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < _errors.Length; index++)
        {
            var error = _errors[index];
            yield return error;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}