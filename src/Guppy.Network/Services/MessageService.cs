using Guppy.Threading;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    internal sealed class MessageService : Broker, IMessageService
    {
        private readonly INetTarget _target;
        private readonly NetScope _scope;

        public MessageService(
            INetTarget target,
            NetScope scope)
        {
            _target = target;
            _scope = scope;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public NetOutgoingMessage<T> CreateOutgoing<T>(in T content)
        {
            return _scope.Outgoing.Create<T>(_target, in content);
        }

        public void ProcessIncoming(NetIncomingMessage message)
        {
            this.Publish(message);

            foreach(NetDeserialized data in message.Data)
            {
                this.Publish(data);
            }
        }
    }
}
