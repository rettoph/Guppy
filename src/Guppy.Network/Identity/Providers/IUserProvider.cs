using Guppy.Network.Identity.Claims;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Identity.Providers
{
    public interface IUserProvider : IEnumerable<User>
    {
        event OnEventDelegate<IUserProvider, User>? OnUserConnected;
        event OnEventDelegate<IUserProvider, User>? OnUserDisconnected;

        public User? Current { get; internal set; }

        IEnumerable<NetPeer> Peers { get;}

        User Update(int id, params Claim[] claims);
        User Update(int id, NetPeer? peer, params Claim[] claims);
        User UpdateOrCreate(int id, params Claim[] claims);
        User UpdateOrCreate(int id, NetPeer? peer, params Claim[] claims);
        User Get(int id);
        bool TryGet(int id, [MaybeNullWhen(false)] out User user);
        void Add(User user);
        void Remove(int id);
    }
}
