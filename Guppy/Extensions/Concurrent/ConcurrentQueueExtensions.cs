using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Concurrent
{
    public static class ConcurrentQueueExtensions
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T instance;

            while (!queue.IsEmpty)
                queue.TryDequeue(out instance);
        }
    }
}
