using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Delegates
{
    public delegate T NetDeserializeDelegate<T>(NetDataReader reader);
}