#region

using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

#endregion

namespace RustRcon.Utility
{
    /// <summary>
    ///     A dictionary which returns null for non-existant keys and removes keys when setting an index to null.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Hash<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;

        public Hash()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public Hash(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                {
                    return value;
                }

                if (typeof(TValue).IsValueType)
                {
                    return (TValue)Activator.CreateInstance(typeof(TValue));
                }

                return default(TValue);
            }

            set
            {
                if (value == null)
                {
                    _dictionary.Remove(key);
                }
                else
                {
                    _dictionary[key] = value;
                }
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;
        public int Count => _dictionary.Count;
        public bool IsReadOnly => _dictionary.IsReadOnly;

        [MustDisposeResource]
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) => _dictionary.CopyTo(array, index);

        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

        public void Add(TKey key, TValue value) => _dictionary.Add(key, value);

        public void Add(KeyValuePair<TKey, TValue> item) => _dictionary.Add(item);

        public bool Remove(TKey key) => _dictionary.Remove(key);

        public bool Remove(KeyValuePair<TKey, TValue> item) => _dictionary.Remove(item);

        public void Clear() => _dictionary.Clear();

        [MustDisposeResource]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}