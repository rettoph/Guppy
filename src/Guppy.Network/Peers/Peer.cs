using Guppy.Network.Constants;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Providers;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Peers
{
    public class Peer : IDisposable
    {
        protected readonly INetScopeProvider scopes;
        protected readonly INetMessageProvider messages;
        protected readonly EventBasedNetListener listener;
        protected readonly NetManager manager;

        public readonly NetScope Scope;

        public IUserProvider Users { get; }

        public User? CurrentUser { get; protected set; }

        public Peer(
            INetScopeProvider scopes,
            INetMessageProvider messages,
            IUserProvider users,
            EventBasedNetListener listener,
            NetManager manager, 
            NetScope scope)
        {
            this.scopes = scopes;
            this.messages = messages;
            this.listener = listener;
            this.manager = manager;

            this.Scope = scope;
            this.Users = users;

            this.listener.NetworkReceiveEvent += this.HandleNetworkReceiveEvent;
        }

        public void Dispose()
        {
            this.listener.NetworkReceiveEvent -= this.HandleNetworkReceiveEvent;
        }

        protected virtual void Start()
        {
            this.Scope.Start(NetScopeConstants.PeerScopeId);
        }

        public void Flush()
        {
            this.manager.PollEvents();

            this.Scope.Flush();
        }

        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            while(!reader.EndOfData)
            {
                var message = this.messages.Read(reader);

                this.scopes.Enqueue(message);
            }
        }
    }
}
