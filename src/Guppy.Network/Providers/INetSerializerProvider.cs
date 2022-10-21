using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface INetSerializerProvider : IEnumerable<INetSerializer>
    {
        INetSerializer<T> Get<T>()
            where T : notnull;

        INetSerializer Get(Type type);

        INetSerializer Get(INetId id);
    }
}
