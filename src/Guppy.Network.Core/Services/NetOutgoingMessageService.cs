using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using Guppy.Network.Utilities;
using Guppy.DependencyInjection;
using Guppy.Network.Contexts;
using Guppy.Network.Interfaces;
using System.Linq;

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
        protected override void PreInitialize(ServiceProvider provider)
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
        public NetOutgoingMessage CreateMessage(IPipe pipe, NetOutgoingMessageContext context, NetConnection recipient = null, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter = null)
        {
            var container = new NetOutgoingMessageContainer(pipe, context, _peer.CreateMessage(), recipient, filter);
            _containers.Enqueue(container);

            return container.Message;
        }

        public void Flush()
        {
            while(_containers.Any())
            {
                _container = _containers.Dequeue();

                
                
                if (_container.Recipient != default)
                { // Broadcast to recipient...
                    _peer.SendMessage(
                        msg: _container.Message,
                        recipient: _container.Recipient,
                        method: _container.Context.Method,
                        sequenceChannel: _container.Context.SequenceChannel);
                }
                else if(_container.Pipe.Users.Connections.Any())
                { // Broadcast to channel...
                    _users.Clear();

                    if(_container.Filter == default)
                    { // Broadcast to all...
                        _users.AddRange(_container.Pipe.Users.Connections);
                    }
                    else
                    { // Brodcast to filtered users...
                        _users.AddRange(_container.Filter(_container.Pipe.Users.Connections));

                        if (!_users.Any())
                            return;
                    }

                    _peer.SendMessage(
                        msg: _container.Message,
                        recipients: _container.Pipe.Users.Connections,
                        method: _container.Context.Method,
                        sequenceChannel: _container.Context.SequenceChannel);
                }
            }
        }
        #endregion
    }
}
