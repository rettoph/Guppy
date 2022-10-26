using Guppy.Common.Collections;
using Guppy.Network;
using Guppy.Network.Definitions;
using Guppy.Network.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Factories.NetOutgoingMessageFactories
{
    internal sealed class ServerNetOutgoingMessageFactory : INetOutgoingMessageFactory
    {
        private DoubleDictionary<INetId, Type, NetMessageType> _messages = default!;

        void INetOutgoingMessageFactory.Initialize(DoubleDictionary<INetId, Type, NetMessageType> messages)
        {
            _messages = messages;
        }

        INetOutgoingMessage<T> INetOutgoingMessageFactory.Create<T>(in T body)
        {
            if (_messages[typeof(T)] is NetMessageType<T> type)
            {
                var message = type.CreateOutgoing();
                message.Write(in body);

                return message;
            }

            throw new NotImplementedException();
        }
    }
}
