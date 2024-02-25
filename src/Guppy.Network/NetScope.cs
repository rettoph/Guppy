using Guppy.Messaging;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Services;
using Guppy.Network.Messages;
using Guppy.Network.Peers;
using Guppy.Network.Services;

namespace Guppy.Network
{
    internal sealed class NetScope :
        INetScope,
        ISubscriber<INetOutgoingMessage>,
        ISubscriber<INetIncomingMessage<UserAction>>,
        IDisposable
    {
        private readonly IBus _bus;

        public NetScopeState State { get; set; }

        public byte Id { get; private set; }
        public IPeer? Peer { get; private set; }
        public INetScopeUserService Users { get; }
        public INetMessageService Messages { get; }

        public NetScope(
            INetMessageService messages,
            IBus bus)
        {
            _bus = bus;

            this.Users = new NetScopeUserService();
            this.Messages = messages;

            _bus.Subscribe(this);

            this.Users.OnUserJoined += this.HandleUserJoined;
            this.Users.OnUserLeft += this.HandleUserLeft;
        }

        public void Dispose()
        {
            _bus.Unsubscribe(this);

            this.Users.OnUserJoined -= this.HandleUserJoined;
            this.Users.OnUserLeft -= this.HandleUserLeft;

            this.Users.Dispose();

            if (this.State == NetScopeState.AttachedToPeer)
            {
                this.Peer!.DetachNetScope(this);
            }
        }

        public void AttachPeer(IPeer peer, byte id)
        {
            if (this.State == NetScopeState.AttachedToPeer)
            {
                throw new InvalidOperationException($"{nameof(NetScope)}::{nameof(AttachPeer)} - {nameof(NetScope)} has already been bound to an {nameof(IPeer)} instance.");
            }

            if (peer.TryAttachNetScope(this, id) == false)
            {
                throw new Exception();
            }

            this.Id = id;
            this.Peer = peer;
            this.State = NetScopeState.AttachedToPeer;
            this.Messages.Initialize(this);
        }

        public void DetachPeer()
        {
            if (this.State == NetScopeState.DetachedFromPeer)
            {
                throw new InvalidOperationException($"{nameof(NetScope)}::{nameof(DetachPeer)} - {nameof(NetScope)} is not bound to an {nameof(IPeer)} instance.");
            }

            this.Peer!.DetachNetScope(this);
            this.Peer = null;
        }

        public void Process(in Guid messageId, INetOutgoingMessage message)
        {
            this.Send(message);
        }

        public void Process(in Guid messsageId, INetIncomingMessage<UserAction> message)
        {
            if (this.Peer!.Type != PeerType.Client)
            {
                return;
            }

            User user = this.Peer.Users.UpdateOrCreate(message.Body.Id, message.Body.Claims);

            switch (message.Body.Type)
            {
                case UserActionTypes.UserJoined:
                    this.Users.Add(user);
                    break;
                case UserActionTypes.UserLeft:
                    this.Users.Remove(user);
                    break;
            }
        }

        private void HandleUserJoined(INetScopeUserService sender, User newUser)
        {
            if (this.Peer!.Type != PeerType.Server)
            {
                return;
            }

            // Alert all users of the new user.
            this.Messages.Create(newUser.CreateAction(UserActionTypes.UserJoined, ClaimAccessibility.Public))
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

                this.Messages.Create(oldUser.CreateAction(UserActionTypes.UserJoined, ClaimAccessibility.Public))
                    .AddRecipient(newUser.NetPeer)
                    .Enqueue();
            }
        }

        private void HandleUserLeft(INetScopeUserService sender, User user)
        {
            if (this.Peer!.Type != PeerType.Server)
            {
                return;
            }

            this.Messages.Create(user.CreateAction(UserActionTypes.UserLeft, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();
        }

        public void Enqueue(INetIncomingMessage message)
        {
            _bus.Enqueue(message);
        }

        public void Enqueue(INetOutgoingMessage message)
        {
            _bus.Enqueue(message);
        }

        public void Send(INetOutgoingMessage message)
        {
            this.Peer!.Send(message);
        }
    }
}
