using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetMessage<TBody> : IEnumerable<NetDatum>, INetMessage
    {
        public readonly NetMessageType Type;

        public byte ScopeId;
        public TBody Body;

        public abstract IEnumerable<NetDatum> Data { get; }

        byte INetMessage.ScopeId => this.ScopeId;

        internal NetMessage(NetMessageType type)
        {
            this.Type = type;

            this.Body = default!;
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
