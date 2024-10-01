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
            get => _peer!;
            set
            {
                if (_peer?.Id == value?.Id)
                {
                    return;
                }

                _peer = value;
                _user = null;
            }
        }

        public IUser User => _user ??= _users.GetByNetPeer(_peer!);
    }
}
