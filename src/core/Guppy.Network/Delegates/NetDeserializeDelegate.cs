using LiteNetLib.Utils;

namespace Guppy.Network.Delegates
{
    public delegate T NetDeserializeDelegate<T>(NetDataReader reader);
}
