using Guppy.Network.Security.Messages;
using Guppy.Network.Security.Structs;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.Providers
{
    public interface IUserProvider : IEnumerable<User>
    {
        bool TryGet(int id, [MaybeNullWhen(false)] out User user);
        User UpdateOrCreate(int id, IEnumerable<Claim> claims);
        User UpdateOrCreate(int id, IEnumerable<Claim> claims, NetPeer? peer);
    }
}
