using Guppy.Network.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetMessage<THeader> : IEnumerable<NetDatum>, INetMessage
    {
        public readonly NetMessageType Type;

        public byte ScopeId;
        public THeader Header;

        public abstract IEnumerable<NetDatum> Data { get; }

        byte INetMessage.ScopeId => this.ScopeId;

        internal NetMessage(NetMessageType type)
        {
            this.Type = type;

            this.Header = default!;
        }

        public abstract IEnumerator<NetDatum> GetEnumerator();
        public abstract void Recycle();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            this.Recycle();
        }
    }
}
