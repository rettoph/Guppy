using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Services;
using Guppy.Network.Messages;

namespace Guppy.Network.Groups
{
    internal abstract class BaseNetGroup :
        INetGroup,
        IDisposable
    {
        public byte Id { get; private set; }
        public IPeer Peer { get; private set; }
        public INetScopeUserService Users { get; }
        public INetScope Scope { get; private set; }

        public BaseNetGroup(byte id, IPeer peer)
        {
            Id = id;
            Peer = peer;
            Users = new NetScopeUserService();
            Scope = null!;

            Users.OnUserJoined += HandleUserJoined;
            Users.OnUserLeft += HandleUserLeft;

            Attach(this.Peer.DefaultNetScope);
        }

        public void Dispose()
        {
            Users.OnUserJoined -= HandleUserJoined;
            Users.OnUserLeft -= HandleUserLeft;

            Users.Dispose();
        }

        public void Attach(INetScope scope)
        {
            if (Scope is not null)
            {
                Detach();
            }

            Scope = scope;
            Scope.Add(this);
        }

        public void Detach()
        {
            if (Scope is null)
            {
                throw new InvalidOperationException($"{nameof(BaseNetGroup)}::{nameof(Detach)} - {nameof(BaseNetGroup)} is not bound to an {nameof(INetScope)} instance.");
            }

            Scope.Remove(this);
            Scope = null!;
        }

        public INetOutgoingMessage<T> CreateMessage<T>(in T body) where T : notnull
        {
            return Peer.Messages.Create(this, body);
        }

        private void HandleUserJoined(INetScopeUserService sender, User newUser)
        {
            if (Peer!.Type != PeerType.Server)
            {
                return;
            }

            // Alert all users of the new user.
            CreateMessage(newUser.CreateAction(UserActionTypes.UserJoined, ClaimAccessibility.Public))
                .AddRecipients(Users.Peers)
                .Enqueue();

            if (newUser.NetPeer is null)
            {
                return;
            }

            // Alert the new user of all existing users.
            foreach (User oldUser in Users)
            {
                if (oldUser.Id == newUser.Id)
                {
                    continue;
                }

                CreateMessage(oldUser.CreateAction(UserActionTypes.UserJoined, ClaimAccessibility.Public))
                    .AddRecipient(newUser.NetPeer)
                    .Enqueue();
            }
        }

        private void HandleUserLeft(INetScopeUserService sender, User user)
        {
            if (Peer!.Type != PeerType.Server)
            {
                return;
            }

            CreateMessage(user.CreateAction(UserActionTypes.UserLeft, ClaimAccessibility.Public))
                .AddRecipients(Users.Peers)
                .Enqueue();
        }

        void INetGroup.Process(INetIncomingMessage<UserAction> message)
        {
            Process(message);
        }

        protected virtual void Process(INetIncomingMessage<UserAction> message)
        {
        }
    }
}
