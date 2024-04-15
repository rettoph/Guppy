using LiteNetLib;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Network.Identity.Services
{
    public interface INetScopeUserService : IEnumerable<User>, IDisposable
    {
        IEnumerable<NetPeer> Peers { get; }

        event OnEventDelegate<INetScopeUserService, User>? OnUserJoined;
        event OnEventDelegate<INetScopeUserService, User>? OnUserLeft;

        User Get(int id);
        bool TryGet(int id, [MaybeNullWhen(false)] out User user);
        void Add(User user);
        void Remove(User user);
    }
}
