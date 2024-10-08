﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Guppy.Core.Common.Collections
{
    public class DoubleDictionary<TKey1, TKey2, TValue>
        where TKey1 : notnull
        where TKey2 : notnull
    {
        private readonly Dictionary<TKey1, TValue> _dic1;
        private readonly Dictionary<TKey2, TValue> _dic2;

        public TValue this[TKey1 key] => _dic1[key];
        public TValue this[TKey2 key] => _dic2[key];

        public ICollection<TValue> Values => _dic1.Values;
        public ICollection<TKey1> Keys1 => _dic1.Keys;
        public ICollection<TKey2> Keys2 => _dic2.Keys;

        public DoubleDictionary()
        {
            _dic1 = [];
            _dic2 = [];
        }

        public DoubleDictionary(int capacity)
        {
            _dic1 = new Dictionary<TKey1, TValue>(capacity);
            _dic2 = new Dictionary<TKey2, TValue>(capacity);
        }

        public DoubleDictionary(IEnumerable<(TKey1, TKey2, TValue)> items) : this(items.Count())
        {
            foreach (var kkv in items)
            {
                this.TryAdd(kkv.Item1, kkv.Item2, kkv.Item3);
            }
        }

        public bool TryAdd(TKey1 key1, TKey2 key2, TValue value)
        {
            if (_dic1.TryAdd(key1, value))
            {
                if (_dic2.TryAdd(key2, value))
                {
                    return true;
                }

                _dic1.Remove(key1);
            }

            return false;
        }

        public bool TryGet(TKey1 key1, [MaybeNullWhen(false)] out TValue value)
        {
            return _dic1.TryGetValue(key1, out value);
        }

        public bool TryGet(TKey2 key2, [MaybeNullWhen(false)] out TValue value)
        {
            return _dic2.TryGetValue(key2, out value);
        }

        public ref TValue TryGetRef(TKey1 key, out bool isNullRef)
        {
            ref TValue value = ref CollectionsMarshal.GetValueRefOrNullRef(_dic1, key);
            isNullRef = Unsafe.IsNullRef(ref value);
            return ref value;
        }

        public ref TValue TryGetRef(TKey2 key, out bool isNullRef)
        {
            ref TValue value = ref CollectionsMarshal.GetValueRefOrNullRef(_dic2, key);
            isNullRef = Unsafe.IsNullRef(ref value);
            return ref value;
        }

        public bool Remove(TKey1 key1, TKey2 key2)
        {
            return _dic1.Remove(key1) && _dic2.Remove(key2);
        }

        public bool Remove(TKey1 key1, TKey2 key2, [MaybeNullWhen(false)] out TValue removed)
        {
            return _dic1.Remove(key1, out removed) && _dic2.Remove(key2);
        }
    }
}
