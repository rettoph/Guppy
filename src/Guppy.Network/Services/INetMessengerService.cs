using Guppy.Network.Components;
using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public interface INetMessengerService : IEnumerable<NetMessenger>
    {
        NetMessenger Get(ushort id);

        bool TryGet(ushort id, [MaybeNullWhen(false)] out NetMessenger room);

        internal void Add(NetMessenger messenger);
        internal void Remove(NetMessenger messenger);
    }
}
