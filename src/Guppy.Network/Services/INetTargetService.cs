using Guppy.Services;
using Minnow.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public interface INetTargetService : ICollectionService<ushort, INetTarget>
    {
        Map<Type, uint> Map { get; }

        bool TryCreate(uint hash, [MaybeNullWhen(false)] out INetTarget target);

        internal bool TryAdd(INetTarget target);
        internal bool TryRemove(INetTarget target);
    }
}
