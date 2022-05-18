using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    internal sealed class NetScopeIncomingMessageService : INetScopeIncomingMessageService
    {
        private Queue<NetIncomingMessage> _messages;
        private INetTargetService _targets;

        public NetScopeIncomingMessageService(INetTargetService targets)
        {
            _messages = new Queue<NetIncomingMessage>();
            _targets = targets;
        }

        public void Enqueue(NetIncomingMessage message)
        {
            _messages.Enqueue(message);
        }

        public void Read()
        {
            while (_messages.TryDequeue(out NetIncomingMessage? im))
            {
                if(_targets.TryGet(im.TargetNetId, out INetTarget? target))
                {
                    target.Messages.ProcessIncoming(im);
                }
                else
                {
                    // This should log gracefully
                    throw new Exception();
                }
            }
        }
    }
}
