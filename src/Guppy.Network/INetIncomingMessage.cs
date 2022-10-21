using Guppy.Common;
using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Guppy.Network
{
    public interface INetIncomingMessage : IMessage, INetMessage, IRecyclable
    {
        object Body { get; }
        IEnumerable<object> Data { get; }
        NetMessageType Type { get; }

        public void Read(NetDataReader reader);
        INetIncomingMessage Enqueue();
    }

    public interface INetIncomingMessage<T> : INetIncomingMessage
        where T : notnull
    {
        new T Body { get; }

        new NetMessageType<T> Type { get; }

        new INetIncomingMessage<T> Enqueue();
    }
}
