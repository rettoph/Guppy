using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Identity.Services
{
    public interface IUserService : IDisposable
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
