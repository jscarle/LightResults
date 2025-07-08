using System.Collections;

namespace LightResults.Common;

/// <summary>A minimal read-only dictionary optimized for a single key/value pair.</summary>
/// <typeparam name="TKey">Dictionary key type.</typeparam>
/// <typeparam name="TValue">Dictionary value type.</typeparam>
internal sealed class SingleItemReadOnlyDictionary<TKey, TValue>(TKey itemKey, TValue itemValue) : IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    public int Count => 1;

    public TValue this[TKey key] => EqualityComparer<TKey>.Default.Equals(key, itemKey) ? itemValue : throw new KeyNotFoundException();

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => new KeyEnumerable(itemKey);

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => new ValueEnumerable(itemValue);

    public bool ContainsKey(TKey key)
    {
        return EqualityComparer<TKey>.Default.Equals(key, itemKey);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (ContainsKey(key))
        {
            value = itemValue;
            return true;
        }

        value = default!;
        return false;
    }

    private Enumerator GetEnumerator()
    {
        return new Enumerator(itemKey, itemValue);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        return GetEnumerator();
    }

    private struct Enumerator(TKey key, TValue value) : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private bool _moved = false;

        public KeyValuePair<TKey, TValue> Current { get; } = new(key, value);

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

    private struct KeyEnumerable(TKey key) : IEnumerable<TKey>, IEnumerator<TKey>
    {
        private bool _moved = false;

        IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public TKey Current => key;

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

    private struct ValueEnumerable(TValue value) : IEnumerable<TValue>, IEnumerator<TValue>
    {
        private bool _moved = false;

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public TValue Current => value;

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
