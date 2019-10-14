using Guppy.Utilities.Delegaters;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.Concurrent;
using System.Linq;

namespace Guppy.Network.Utilitites.Delegaters
{
    public sealed class MessageDelegater : CustomDelegater<String, NetIncomingMessage>
    {
        private ILogger _logger;
        private ConcurrentQueue<NetIncomingMessage> _messages;
        private NetIncomingMessage _im;

        public MessageDelegater(ILogger logger)
        {
            _logger = logger;
            _messages = new ConcurrentQueue<NetIncomingMessage>();
        }

        protected override void Invoke<T>(object sender, string key, T arg)
        {
#if DEBUG
            try
            {
                base.Invoke(sender, key, arg);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogWarning($"Unhandled Message recieved => '{key}'");
            }
#else
            base.Invoke(sender, key, arg);
#endif
        }

        /// <summary>
        /// Enqueue an incoming message to be read.
        /// </summary>
        /// <param name="im"></param>
        public void Enqueue(NetIncomingMessage im)
        {
            _messages.Enqueue(im);
        }

        /// <summary>
        /// Read all incoming messages.
        /// </summary>
        public void Flush()
        {
            while (_messages.Any())
                if(_messages.TryDequeue(out _im))
                    this.Invoke<NetIncomingMessage>(this, _im.ReadString(), _im);
        }

        public override void Dispose()
        {
            base.Dispose();

            _messages.Clear();
        }
    }
}
