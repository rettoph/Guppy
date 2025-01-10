using System.Diagnostics.CodeAnalysis;
using LiteNetLib;

namespace Guppy.Core.Network.Common.Services
{
    public interface INetScopeUserService : IEnumerable<IUser>, IDisposable
    {
        IEnumerable<NetPeer> Peers { get; }

        event OnEventDelegate<INetScopeUserService, IUser>? OnUserJoined;
        event OnEventDelegate<INetScopeUserService, IUser>? OnUserLeft;

        IUser Get(int id);
        bool TryGet(int id, [MaybeNullWhen(false)] out IUser user);
        void Add(IUser user);
        void Remove(IUser user);
    }
}