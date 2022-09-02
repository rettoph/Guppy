using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface INetMessageProvider
    {
        INetIncomingMessage Read(NetDataReader reader);

        NetOutgoingMessage<TBody> Create<TBody>(in TBody body, NetScope scope);
    }
}
