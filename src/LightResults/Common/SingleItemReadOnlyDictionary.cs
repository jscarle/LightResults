using System.Collections;
using System.Collections.Generic;

namespace LightResults.Common;

/// <summary>
/// A minimal read-only dictionary optimized for a single key/value pair.
/// </summary>
/// <typeparam name="TKey">Dictionary key type.</typeparam>
/// <typeparam name="TValue">Dictionary value type.</typeparam>
internal sealed class SingleItemReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly TKey _key;
    private readonly TValue _value;

    public SingleItemReadOnlyDictionary(TKey key, TValue value)
    {
        _key = key;
        _value = value;
    }

    public TValue this[TKey key] => EqualityComparer<TKey>.Default.Equals(key, _key)
        ? _value
        : throw new KeyNotFoundException();

    public int Count => 1;

    public bool ContainsKey(TKey key) => EqualityComparer<TKey>.Default.Equals(key, _key);

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (ContainsKey(key))
        {
            value = _value;
            return true;
        }

        value = default!;
        return false;
    }

    public Enumerator GetEnumerator() => new(_key, _value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => new KeyEnumerable(_key);

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => new ValueEnumerable(_value);

    internal struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private readonly KeyValuePair<TKey, TValue> _pair;
        private bool _moved;

        public Enumerator(TKey key, TValue value)
        {
            _pair = new KeyValuePair<TKey, TValue>(key, value);
            _moved = false;
        }

        public KeyValuePair<TKey, TValue> Current => _pair;

        object IEnumerator.Current => _pair;

        public bool MoveNext()
        {
            if (_moved)
                return false;

            _moved = true;
            return true;
        }

        public void Reset() => _moved = false;

        public void Dispose()
        {
        }
    }

    private struct KeyEnumerable : IEnumerable<TKey>, IEnumerator<TKey>
    {
        private readonly TKey _key;
        private bool _moved;

        public KeyEnumerable(TKey key)
        {
            _key = key;
            _moved = false;
        }

        public KeyEnumerable GetEnumerator() => this;

        IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;

        public TKey Current => _key;

        object IEnumerator.Current => _key!;

        public bool MoveNext()
        {
            if (_moved)
                return false;

            _moved = true;
            return true;
        }

        public void Reset() => _moved = false;

        public void Dispose()
        {
        }
    }

    private struct ValueEnumerable : IEnumerable<TValue>, IEnumerator<TValue>
    {
        private readonly TValue _value;
        private bool _moved;

        public ValueEnumerable(TValue value)
        {
            _value = value;
            _moved = false;
        }

        public ValueEnumerable GetEnumerator() => this;

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;

        public TValue Current => _value;

        object IEnumerator.Current => _value!;

        public bool MoveNext()
        {
            if (_moved)
                return false;

            _moved = true;
            return true;
        }

        public void Reset() => _moved = false;

        public void Dispose()
        {
        }
    }
}
