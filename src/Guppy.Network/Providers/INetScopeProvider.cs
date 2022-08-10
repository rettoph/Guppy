using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface INetScopeProvider
    {
        NetScope Get(byte id);
        bool TryGet(byte id, [MaybeNullWhen(false)] out NetScope scope);

        void Enqueue(INetIncomingMessage message);
        void Enqueue(INetOutgoingMessage message);

        NetScope Create();
    }
}
