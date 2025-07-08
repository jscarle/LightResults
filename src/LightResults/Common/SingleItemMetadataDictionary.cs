using System.Collections;

namespace LightResults.Common;

/// <summary>A minimal read-only dictionary optimized for a single metadata entry.</summary>
internal sealed class SingleItemMetadataDictionary : IReadOnlyDictionary<string, object?>
{
    private readonly string _key;
    private readonly object? _value;

    public SingleItemMetadataDictionary(string key, object? value)
    {
        _key = key;
        _value = value;
    }

    public int Count => 1;

    public object? this[string key] => string.Equals(key, _key, StringComparison.Ordinal) ? _value : throw new KeyNotFoundException();

    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => new KeyEnumerable(_key);

    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => new ValueEnumerable(_value);

    public bool ContainsKey(string key)
    {
        return string.Equals(key, _key, StringComparison.Ordinal);
    }

    public bool TryGetValue(string key, out object? value)
    {
        if (ContainsKey(key))
        {
            value = _value;
            return true;
        }

        value = null;
        return false;
    }

    private Enumerator GetEnumerator()
    {
        return new Enumerator(_key, _value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator()
    {
        return GetEnumerator();
    }

    private struct Enumerator(string key, object? value) : IEnumerator<KeyValuePair<string, object?>>
    {
        private bool _moved;

        public KeyValuePair<string, object?> Current { get; } = new(key, value);

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_moved)
                return false;

            _moved = true;
            return true;
        }

        public void Reset()
        {
            _moved = false;
        }

        public void Dispose()
        {
        }
    }

    private struct KeyEnumerable(string key) : IEnumerable<string>, IEnumerator<string>
    {
        private bool _moved;

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public string Current => key;

        object IEnumerator.Current => key;

        public bool MoveNext()
        {
            if (_moved)
                return false;

            _moved = true;
            return true;
        }

        public void Reset()
        {
            _moved = false;
        }

        public void Dispose()
        {
        }
    }

    private struct ValueEnumerable(object? value) : IEnumerable<object?>, IEnumerator<object?>
    {
        private bool _moved;

        IEnumerator<object?> IEnumerable<object?>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public object? Current => value;

        object IEnumerator.Current => value!;

        public bool MoveNext()
        {
            if (_moved)
                return false;

            _moved = true;
            return true;
        }

        public void Reset()
        {
            _moved = false;
        }

        public void Dispose()
        {
        }
    }
}
