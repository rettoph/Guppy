using Guppy.Common.Collections;
using Guppy.Network;
using Guppy.Network.Definitions;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Factories.NetOutgoingMessageFactories
{
    internal sealed class ClientNetOutgoingMessageFactory : INetOutgoingMessageFactory
    {
        private IUserProvider _users;
        private DoubleDictionary<INetId, Type, NetMessageType> _messages = default!;

        public ClientNetOutgoingMessageFactory(IUserProvider users)
        {
            _users = users;
        }

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

                // TODO: This should be checked for null at some point :o)
                if(_users.Current?.NetPeer is not null)
                {
                    message.AddRecipient(_users.Current.NetPeer);
                }
                
                return message;
            }

            throw new NotImplementedException();
        }
    }
}
