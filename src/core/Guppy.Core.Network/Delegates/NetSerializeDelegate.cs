using LiteNetLib.Utils;

namespace Guppy.Core.Network.Delegates
{
    public delegate void NetSerializeDelegate<T>(NetDataWriter writer, in T instance);
}
