using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Guppy.Core.Common.Collections
{
    public sealed class DictionaryQueue<TKey, TValue>
        where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> _dict = [];
        private readonly Queue<TKey> _queue = new();

        public TValue this[TKey key] => this._dict[key];

        public bool TryEnqueue(TKey key, TValue value)
        {
            if (!this._dict.TryAdd(key, value))
            {
                return false;
            }

            this._queue.Enqueue(key);
            return true;
        }

        public bool TryDequeue([MaybeNullWhen(false)] out TValue value)
        {
            if (!this._queue.TryDequeue(out TKey? key))
            {
                value = default;
                return false;
            }

            return this._dict.Remove(key, out value);
        }

        public bool TryDequeue([MaybeNullWhen(false)] out TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (!this._queue.TryDequeue(out key))
            {
                value = default;
                return false;
            }

            return this._dict.Remove(key, out value);
        }

        public bool TryGet(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return this._dict.TryGetValue(key, out value);
        }

        public bool TryPeek([MaybeNullWhen(false)] out TValue value)
        {
            if (this._queue.TryPeek(out TKey? key))
            {
                value = this._dict[key];
                return true;
            }

            value = default!;
            return false;
        }

        public ref TValue? GetOrEnqueue(TKey key, out bool exists)
        {
            ref TValue? value = ref CollectionsMarshal.GetValueRefOrAddDefault(this._dict, key, out exists);

            if (exists == false)
            {
                this._queue.Enqueue(key);
            }

            return ref value;
        }

        public void Clear()
        {
            this._queue.Clear();
            this._dict.Clear();
        }
    }
}