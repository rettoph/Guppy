using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Common.Collections
{
    public class Map<T1, T2> : IEnumerable<(T1, T2)>
        where T1 : notnull
        where T2 : notnull
    {
        private readonly Dictionary<T1, T2> _forward;
        private readonly Dictionary<T2, T1> _reverse;

        public ICollection<T1> Values1 => _forward.Keys;
        public ICollection<T2> Values2 => _reverse.Keys;

        public T2 this[T1 key]
        {
            get => _forward[key];
            set
            {
                _forward[key] = value;
                _reverse[value] = key;
            }
        }

        public T1 this[T2 key]
        {
            get => _reverse[key];
            set
            {
                _reverse[key] = value;
                _forward[value] = key;
            }
        }

        public int Count => _forward.Count;

        public Map()
        {
            _forward = new Dictionary<T1, T2>();
            _reverse = new Dictionary<T2, T1>();
        }

        public Map(IEnumerable<T1> first, IEnumerable<T2> second) : this(first.Zip(second))
        {
        }

        public Map(IEnumerable<(T1, T2)> values)
        {
            _forward = values.ToDictionary(
                keySelector: v => v.Item1,
                elementSelector: v => v.Item2);

            _reverse = values.ToDictionary(
                keySelector: v => v.Item2,
                elementSelector: v => v.Item1);
        }

        public bool TryAdd(T1 t1, T2 t2)
        {
            if (!_forward.TryAdd(t1, t2))
            {
                return false;
            }

            if (!_reverse.TryAdd(t2, t1))
            {
                _forward.Remove(t1);
                return false;
            }

            return true;
        }

        public bool TryRemove(T1 t1)
        {
            if (!_forward.Remove(t1, out var t2))
            {
                return false;
            }

            if (!_reverse.Remove(t2))
            {
                _forward.Add(t1, t2);
                return false;
            }

            return true;
        }

        public bool TryRemove(T2 t2)
        {
            if (!_reverse.Remove(t2, out var t1))
            {
                return false;
            }

            if (!_forward.Remove(t1))
            {
                _reverse.Add(t2, t1);
                return false;
            }

            return true;
        }

        public bool TryGet(T1 key1, [MaybeNullWhen(false)] out T2 value2)
        {
            return _forward.TryGetValue(key1, out value2);
        }

        public bool TryGet(T2 key2, [MaybeNullWhen(false)] out T1 value1)
        {
            return _reverse.TryGetValue(key2, out value1);
        }

        public IEnumerator<(T1, T2)> GetEnumerator()
        {
            foreach (KeyValuePair<T1, T2> kvp in _forward)
            {
                yield return (kvp.Key, kvp.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
