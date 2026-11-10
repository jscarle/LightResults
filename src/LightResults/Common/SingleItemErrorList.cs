using System.Collections;
using System.Runtime.CompilerServices;

namespace LightResults.Common;

/// <summary>A minimal read-only list optimized for a single error.</summary>
internal sealed class SingleItemErrorList : IReadOnlyList<IError>
{
    private readonly IError _error;

    public SingleItemErrorList(IError error)
    {
        _error = error;
    }

    public int Count => 1;

    public IError this[int index] => index == 0 ? _error : throw new ArgumentOutOfRangeException(nameof(index));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Enumerator GetEnumerator()
    {
        return new Enumerator(_error);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<IError> IEnumerable<IError>.GetEnumerator()
    {
        return GetEnumerator();
    }

    private struct Enumerator(IError error) : IEnumerator<IError>
    {
        private bool _moved;

        public IError Current { get; } = error;

        object IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (_moved)
                return false;

            _moved = true;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            _moved = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
        }
    }
}
