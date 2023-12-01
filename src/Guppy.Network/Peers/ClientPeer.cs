using Autofac;
using Guppy.Common;
using Guppy.Messaging;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Messages;
using LiteNetLib;

namespace Guppy.Network.Peers
{
    public class ClientPeer : Peer, ISubscriber<INetIncomingMessage<UserAction>>
    {
        private NetPeer? _peer;

        public override PeerType Type => PeerType.Client;

        public ClientPeer(ILifetimeScope scope) : base(scope)
        {
        }

        public new void Start()
        {
            base.Start();

            this.Manager.Start();

            this.NetScope.Bus.Subscribe(this);
        }

        public void Connect(string address, int port, params Claim[] claims)
        {
            var user = new User(-1, claims);
            var action = user.CreateAction(UserAction.Actions.ConnectionRequest, ClaimAccessibility.Protected);

            using(var request = this.NetScope.Messages.Create(in action))
            {
                _peer = this.Manager.Connect(address, port, request.Writer);
            }
        }

        public void Process(in Guid messsageId, INetIncomingMessage<UserAction> message)
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
