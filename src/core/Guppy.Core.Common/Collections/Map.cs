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

        public ICollection<T1> Values1 => this._forward.Keys;
        public ICollection<T2> Values2 => this._reverse.Keys;

        public T2 this[T1 key]
        {
            get => this._forward[key];
            set
            {
                this._forward[key] = value;
                this._reverse[value] = key;
            }
        }

        public T1 this[T2 key]
        {
            get => this._reverse[key];
            set
            {
                this._reverse[key] = value;
                this._forward[value] = key;
            }
        }

        public int Count => this._forward.Count;

        public Map()
        {
            this._forward = [];
            this._reverse = [];
        }

        public Map(IEnumerable<T1> first, IEnumerable<T2> second) : this(first.Zip(second))
        {
        }

        public Map(IEnumerable<(T1, T2)> values)
        {
            this._forward = values.ToDictionary(
                keySelector: v => v.Item1,
                elementSelector: v => v.Item2);

            this._reverse = values.ToDictionary(
                keySelector: v => v.Item2,
                elementSelector: v => v.Item1);
        }

        public bool TryAdd(T1 t1, T2 t2)
        {
            if (!this._forward.TryAdd(t1, t2))
            {
                return false;
            }

            if (!this._reverse.TryAdd(t2, t1))
            {
                this._forward.Remove(t1);
                return false;
            }

            return true;
        }

        public bool TryRemove(T1 t1)
        {
            if (!this._forward.Remove(t1, out var t2))
            {
                return false;
            }

            if (!this._reverse.Remove(t2))
            {
                this._forward.Add(t1, t2);
                return false;
            }

            return true;
        }

        public bool TryRemove(T2 t2)
        {
            if (!this._reverse.Remove(t2, out var t1))
            {
                return false;
            }

            if (!this._forward.Remove(t1))
            {
                this._reverse.Add(t2, t1);
                return false;
            }

            return true;
        }

        public bool TryGet(T1 key1, [MaybeNullWhen(false)] out T2 value2)
        {
            return this._forward.TryGetValue(key1, out value2);
        }

        public bool TryGet(T2 key2, [MaybeNullWhen(false)] out T1 value1)
        {
            return this._reverse.TryGetValue(key2, out value1);
        }

        public IEnumerator<(T1, T2)> GetEnumerator()
        {
            foreach (KeyValuePair<T1, T2> kvp in this._forward)
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