using LiteNetLib.Utils;

namespace Guppy.Core.Network.Delegates
{
    public delegate T NetDeserializeDelegate<T>(NetDataReader reader);
}
