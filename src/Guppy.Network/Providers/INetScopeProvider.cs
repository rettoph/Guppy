using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface INetScopeProvider : IEnumerable<NetScope>
    {
        bool TryGet(byte id, [MaybeNullWhen(false)] out NetScope room);

        internal bool TryAdd(NetScope room);
        internal bool TryRemove(NetScope room);
    }
}
