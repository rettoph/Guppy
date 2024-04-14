using LiteNetLib.Utils;

namespace Guppy.Network.Delegates
{
    public delegate void NetSerializeDelegate<T>(NetDataWriter writer, in T instance);
}
