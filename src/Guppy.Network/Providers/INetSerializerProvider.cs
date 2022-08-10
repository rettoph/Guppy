using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface INetSerializerProvider
    {
        NetSerializer<T> Get<T>();
        bool TryGet<T>([MaybeNullWhen(false)] out NetSerializer<T> serializer);

        IEnumerable<NetDatumType> BuildDatumTypes();
    }
}
