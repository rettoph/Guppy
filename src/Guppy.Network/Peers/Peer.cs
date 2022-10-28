using Guppy.Common;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Providers;
using Guppy.Resources;
using Guppy.Resources.Providers;
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
        private readonly ISetting<NetAuthorization> _authorization;

        protected readonly INetScopeProvider scopes;
        protected readonly EventBasedNetListener listener;
        protected readonly NetManager manager;

        public readonly NetScope Scope;

        public IUserProvider Users { get; }

        public NetAuthorization Authorization
        {
            get => _authorization.Value;
            set => _authorization.Value = value;
        }

        public Peer(
            ISettingProvider settings,
            INetScopeProvider scopes,
            IUserProvider users,
            IScoped<NetScope> scope,
            EventBasedNetListener listener,
            NetManager manager)
        {
            this.scopes = scopes;
            this.listener = listener;
            this.manager = manager;

            this.Scope = scope.Instance;
            this.Users = users;

            _authorization = settings.Get<NetAuthorization>();

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

            this.Scope.Bus.Flush();
        }

        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            while(!reader.EndOfData)
            {
                var scopeId = reader.GetByte();
                var scope = this.scopes.Get(scopeId);

                scope.Read(peer, reader).Enqueue();
            }
        }
    }
}
