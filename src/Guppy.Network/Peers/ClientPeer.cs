using Guppy.Common;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using Guppy.Resources.Providers;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Peers
{
    public class ClientPeer : Peer, ISubscriber<NetIncomingMessage<UserAction>>
    {
        private NetPeer? _peer;

        public ClientPeer(
            ISettingProvider settings,
            INetScopeProvider scopes, 
            INetMessageProvider messages, 
            IUserProvider users,
            IScoped<NetScope> scope,
            EventBasedNetListener listener, 
            NetManager manager) : base(settings, scopes, messages, users, scope, listener, manager)
        {
            this.Authorization = NetAuthorization.Slave;
        }

        public new void Start()
        {
            base.Start();

            this.manager.Start();

            this.Scope.Subscribe(this);
        }

        public void Connect(string address, int port, params Claim[] claims)
        {
            var user = new User(-1, claims);
            var action = user.CreateAction(UserAction.Actions.ConnectionRequest, ClaimAccessibility.Protected);

            using(var request = this.Scope.Create(in action))
            {
                _peer = this.manager.Connect(address, port, request.Writer);
            }
        }

        public void Process(in NetIncomingMessage<UserAction> message)
        {
            switch (message.Body.Action)
            {
                case UserAction.Actions.Connected:
                    this.Users.UpdateOrCreate(message.Body.Id, message.Body.Claims);
                    break;
                case UserAction.Actions.CurrentUserConnected:
                    this.Users.Current = this.Users.UpdateOrCreate(message.Body.Id, _peer, message.Body.Claims);
                    break;
            }
        }
    }
}
