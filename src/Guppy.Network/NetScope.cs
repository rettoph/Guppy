using Guppy.Messaging;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Services;
using Guppy.Network.Messages;
using Guppy.Network.Peers;
using Guppy.Network.Services;
using System.Diagnostics;

namespace Guppy.Network
{
    public sealed class NetScope :
        ISubscriber<INetOutgoingMessage>,
        ISubscriber<INetIncomingMessage<UserAction>>,
        IDisposable
    {
        internal byte id;

        public byte Id
        {
            get => this.id;
            set => this.id = value;
        }
        public Peer? Peer { get; private set; }
        public bool Bound { get; private set; }
        public IUserService Users { get; }

        public readonly IBus Bus;

        public readonly INetMessageService Messages;

        public NetScope(
            INetMessageService messages,
            IBus bus)
        {
            this.Users = new UserService();
            this.Bus = bus;
            this.Messages = messages;

            this.Bus.Subscribe(this);

            this.Users.OnUserJoined += this.HandleUserJoined;
            this.Users.OnUserLeft += this.HandleUserLeft;
        }

        public void Dispose()
        {
            this.Bus.Unsubscribe(this);

            this.Users.OnUserJoined -= this.HandleUserJoined;
            this.Users.OnUserLeft -= this.HandleUserLeft;

            this.Users.Dispose();

            if (this.Bound)
            {
                this.Peer!.Unbind(this);
            }
        }

        internal void BindTo(Peer peer, byte id)
        {
            if (this.Bound == true)
            {
                throw new UnreachableException();
            }

            this.Id = id;
            this.Peer = peer;
            this.Bound = true;
            this.Messages.Initialize(this);
        }

        internal void Unbind()
        {
            if (this.Bound == false)
            {
                throw new UnreachableException();
            }

            this.Id = default;
            this.Peer = null;
            this.Bound = false;
        }

        public void Process(in Guid messageId, INetOutgoingMessage message)
        {
            message.Send();
        }

        public void Process(in Guid messsageId, INetIncomingMessage<UserAction> message)
        {
            if (this.Peer!.Type != PeerType.Client)
            {
                return;
            }

            var user = this.Peer.Users.UpdateOrCreate(message.Body.Id, message.Body.Claims);

            switch (message.Body.Action)
            {
                case UserAction.Actions.UserJoined:
                    this.Users.Add(user);
                    break;
                case UserAction.Actions.UserLeft:
                    this.Users.Remove(user);
                    break;
            }
        }

        private void HandleUserJoined(IUserService sender, User newUser)
        {
            if (this.Peer!.Type != PeerType.Server)
            {
                return;
            }

            // Alert all users of the new user.
            this.Messages.Create(newUser.CreateAction(UserAction.Actions.UserJoined, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();

            if (newUser.NetPeer is null)
            {
                return;
            }

            // Alert the new user of all existing users.
            foreach (User oldUser in this.Users)
            {
                if (oldUser.Id == newUser.Id)
                {
                    continue;
                }

                this.Messages.Create(oldUser.CreateAction(UserAction.Actions.UserJoined, ClaimAccessibility.Public))
                    .AddRecipient(newUser.NetPeer)
                    .Enqueue();
            }
        }

        private void HandleUserLeft(IUserService sender, User user)
        {
            if (this.Peer!.Type != PeerType.Server)
            {
                return;
            }

            this.Messages.Create(user.CreateAction(UserAction.Actions.UserLeft, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();
        }
    }
}
