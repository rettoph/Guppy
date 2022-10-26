using Guppy.Common.Collections;
using Guppy.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Factories
{
    public interface INetOutgoingMessageFactory
    {
        internal void Initialize(DoubleDictionary<INetId, Type, NetMessageType> messages);

        INetOutgoingMessage<T> Create<T>(in T body)
            where T : notnull;
    }
}
