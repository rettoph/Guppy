using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Dtos;
using LiteNetLib;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Network.Identity.Services
{
    public interface IUserService : IEnumerable<User>
    {
        event OnEventDelegate<IUserService, User>? OnUserConnected;
        event OnEventDelegate<IUserService, User>? OnUserDisconnected;

        public User? Current { get; internal set; }

        IEnumerable<NetPeer> Peers { get; }

        internal User Create(NetPeer? peer, Claim[] claims, params Claim[] additionalClaims);
        internal User Create(NetPeer? peer, UserDto userDto, params Claim[] additionalClaims);
        internal User Update(int id, IEnumerable<Claim> claims);
        User UpdateOrCreate(UserDto userDto);

        User GetById(int id);
        bool TryGet(int id, [MaybeNullWhen(false)] out User user);

        User GetByNetPeer(NetPeer peer);

        void Add(User user);
        void Remove(int id);
    }
}
