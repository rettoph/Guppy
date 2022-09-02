using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ECS.Services
{
    internal sealed class NetDataIdProvider
    {
        private Queue<ushort> _available;
        private ushort _current;

        public NetDataIdProvider()
        {
            _current = 0;
            _available = new Queue<ushort>();
        }

        public void Reserve(out ushort id)
        {
            if(!_available.TryDequeue(out id))
            {
                id = _current++;
            }
        }

        public void Release(in ushort id)
        {
            _available.Enqueue(id);
        }
    }
}
