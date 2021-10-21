using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using Guppy.Network.Utilities;
using Guppy.DependencyInjection;
using Guppy.Network.Contexts;
using Guppy.Network.Interfaces;
using System.Linq;
using Guppy.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Guppy.Network.Services
{
    /// <summary>
    /// Simple service managing the creation of <see cref="NetOutgoingMessage"/>
    /// instances and the underlying <see cref="NetOutgoingMessageContainer"/>.
    /// </summary>
    public sealed class NetOutgoingMessageService : Service
    {
        #region Private Fields
        private NetPeer _peer;
        private ConcurrentQueue<NetOutgoingMessageContainer> _containers;
        private List<NetConnection> _users;
        #endregion

        #region Internal Properties
        internal Int16 channelId { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            _users = new List<NetConnection>();
            _containers = new ConcurrentQueue<NetOutgoingMessageContainer>();

            provider.Service(out _peer);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _users.Clear();
            _containers.Clear();

            _users = null;
            _containers = null;
            _peer = null;
        }
        #endregion

        #region Helper Methods
        public void CreateMessage(Action<NetOutgoingMessage> writer, NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
        {
            var container = new NetOutgoingMessageContainer(context, _peer.CreateMessage(), recipients);

            writer(container.Message);

            _containers.Enqueue(container);
        }

        public void Flush(out UInt32 flushed, out UInt32 sent)
        {
            flushed = 0;
            sent = 0;

            while(_containers.TryDequeue(out NetOutgoingMessageContainer container))
            {
                flushed++;

                _users.Clear();
                _users.AddRange(container.Recipients);

                if(_users.Count > 0)
                {
                    _peer.SendMessage(
                           msg: container.Message,
                           recipients: _users,
                           method: container.Context.Method,
                           sequenceChannel: container.Context.SequenceChannel);

                    sent++;
                }
            }
        }
        #endregion
    }
}
