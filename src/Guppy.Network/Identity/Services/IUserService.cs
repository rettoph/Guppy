using Guppy.Network.Identity.Claims;
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

        User Create(int id, NetPeer? peer, params Claim[] claims);
        User Update(int id, params Claim[] claims);
        User UpdateOrCreate(int id, params Claim[] claims);
        User GetById(int id);
        bool TryGet(int id, [MaybeNullWhen(false)] out User user);
        void Add(User user);
        void Remove(int id);
    }
}
