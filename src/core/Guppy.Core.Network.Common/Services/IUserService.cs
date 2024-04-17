using Guppy.Core.Network.Common.Dtos;
using LiteNetLib;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Network.Common.Services
{
    public interface IUserService : IEnumerable<IUser>
    {
        event OnEventDelegate<IUserService, IUser>? OnUserConnected;
        event OnEventDelegate<IUserService, IUser>? OnUserDisconnected;

        public IUser Current { get; }

        IEnumerable<NetPeer> Peers { get; }

        IUser UpdateOrCreate(UserDto userDto);

        IUser GetById(int id);
        bool TryGet(int id, [MaybeNullWhen(false)] out IUser user);

        IUser GetByNetPeer(NetPeer peer);

        void Remove(int id);
    }
}
