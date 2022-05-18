using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    internal class NetIdProvider
    {
        private ushort _next;
        private Queue<ushort> _released;

        public NetIdProvider()
        {
            _next = 1;
            _released = new Queue<ushort>();
        }

        /// <summary>
        /// Reserve an id.
        /// </summary>
        /// <returns></returns>
        public ushort Reserve()
        {
            if(_released.Count == 0)
            {
                return _next++;
            }

            return _released.Dequeue();
        }

        /// <summary>
        /// Release a previously reserved id, allowing it to be used again in the future.
        /// </summary>
        /// <param name="id"></param>
        public void Release(ushort id)
        {
            _released.Enqueue(id);
        }
    }
}
