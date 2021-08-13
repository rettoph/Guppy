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
        private Queue<NetOutgoingMessageContainer> _containers;
        private NetOutgoingMessageContainer _container;
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
            _containers = new Queue<NetOutgoingMessageContainer>();

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
        public NetOutgoingMessage CreateMessage(NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
        {
            var container = new NetOutgoingMessageContainer(context, _peer.CreateMessage(), recipients);
            _containers.Enqueue(container);

            return container.Message;
        }

        public void Flush()
        {
            while(_containers.Any())
            {
                _container = _containers.Dequeue();
                _users.Clear();
                _users.AddRange(_container.Recipients);

                if(_users.Any())
                    _peer.SendMessage(
                        msg: _container.Message,
                        recipients: _users,
                        method: _container.Context.Method,
                        sequenceChannel: _container.Context.SequenceChannel);
            }
        }
        #endregion
    }
}
