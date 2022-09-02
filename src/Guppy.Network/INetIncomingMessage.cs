using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public interface INetIncomingMessage : INetMessage
    {
        public void Read(NetDataReader reader);
        INetIncomingMessage Enqueue(INetScopeProvider scopes);
    }
}
