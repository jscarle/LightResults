using System.Collections;
using System.Runtime.CompilerServices;

namespace LightResults.Common;

/// <summary>A minimal read-only dictionary optimized for a single metadata entry.</summary>
internal sealed class SingleItemMetadataDictionary(string key, object? value) : IReadOnlyDictionary<string, object?>
{
    public int Count => 1;

    public object? this[string key1] => string.Equals(key1, key, StringComparison.Ordinal) ? value : throw new KeyNotFoundException();

    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => new KeyEnumerable(key);

    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => new ValueEnumerable(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ContainsKey(string key1)
    {
        return string.Equals(key1, key, StringComparison.Ordinal);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(string key1, out object? value1)
    {
        if (string.Equals(key1, key, StringComparison.Ordinal))
        {
            value1 = value;
            return true;
        }

        value1 = null;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Enumerator GetEnumerator()
    {
        return new Enumerator(key, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator()
    {
        return GetEnumerator();
    }

    private struct Enumerator(string key, object? value) : IEnumerator<KeyValuePair<string, object?>>
    {
        private bool _moved;

        public KeyValuePair<string, object?> Current { get; } = new(key, value);

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

    private struct KeyEnumerable(string key) : IEnumerable<string>, IEnumerator<string>
    {
        private bool _moved;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public string Current => key;

        object IEnumerator.Current => key;

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

    private struct ValueEnumerable(object? value) : IEnumerable<object?>, IEnumerator<object?>
    {
        private bool _moved;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<object?> IEnumerable<object?>.GetEnumerator()
        {
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public object? Current => value;

        object IEnumerator.Current => value!;

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
