using Guppy.Common;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
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
        private NetPeer _peer;

        public ClientPeer(
            INetScopeProvider scopes, 
            INetMessageProvider messages, 
            IUserProvider users,
            EventBasedNetListener listener, 
            NetManager manager, 
            NetScope scope) : base(scopes, messages, users, listener, manager, scope)
        {
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

            using(var request = messages.Write(in action))
            {
                _peer = this.manager.Connect(address, port, request.Writer);
            }
        }

        public void Process(in NetIncomingMessage<UserAction> message)
        {
            switch (message.Header.Action)
            {
                case UserAction.Actions.Connected:
                    this.Users.UpdateOrCreate(message.Header.Id, message.Header.Claims);
                    break;
                case UserAction.Actions.CurrentUserConnected:
                    this.CurrentUser = this.Users.UpdateOrCreate(message.Header.Id, _peer, message.Header.Claims);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
