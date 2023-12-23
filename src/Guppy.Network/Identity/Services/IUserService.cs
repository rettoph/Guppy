using LiteNetLib;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Network.Identity.Services
{
    public interface IUserService : IEnumerable<User>, IDisposable
    {
        IEnumerable<NetPeer> Peers { get; }

        event OnEventDelegate<IUserService, User>? OnUserJoined;
        event OnEventDelegate<IUserService, User>? OnUserLeft;

        User Get(int id);
        bool TryGet(int id, [MaybeNullWhen(false)] out User user);
        void Add(User user);
        void Remove(User user);
    }
}
