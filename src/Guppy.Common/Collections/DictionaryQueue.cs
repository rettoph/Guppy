using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Collections
{
    public sealed class DictionaryQueue<TKey, TValue>
        where TKey : notnull
    {
        private Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();
        private Queue<TKey> _queue = new Queue<TKey>();

        public TValue this[TKey key] => _dict[key];

        public bool TryEnqueue(TKey key, TValue value)
        {
            if(!_dict.TryAdd(key, value))
            {
                return false;
            }

            _queue.Enqueue(key); 
            return true;
        }

        public bool TryDequeue([MaybeNullWhen(false)] out TValue value)
        {
            if(!_queue.TryDequeue(out TKey? key))
            {
                value = default;
                return false;
            }

            return _dict.Remove(key, out value);
        }

        public bool TryGet(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public bool TryPeek([MaybeNullWhen(false)] out TValue value)
        {
            if(_queue.TryPeek(out TKey? key))
            {
                value = _dict[key];
                return true;
            }

            value = default!;
            return false;
        }

        public void Clear()
        {
            _queue.Clear();
            _dict.Clear();
        }
    }
}
