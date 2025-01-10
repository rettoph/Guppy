using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Delegates
{
    public delegate void NetSerializeDelegate<T>(NetDataWriter writer, in T instance);
}