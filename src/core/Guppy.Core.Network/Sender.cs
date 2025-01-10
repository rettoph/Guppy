using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Services;
using LiteNetLib;

namespace Guppy.Core.Network
{
    internal sealed class Sender(IUserService users) : ISender
    {
        private readonly IUserService _users = users;
        private IUser? _user;
        private NetPeer? _peer;

        public NetPeer Peer
        {
            get => this._peer!;
            set
            {
                if (this._peer?.Id == value?.Id)
                {
                    return;
                }

                this._peer = value;
                this._user = null;
            }
        }

        public IUser User => this._user ??= this._users.GetByNetPeer(this._peer!);
    }
}