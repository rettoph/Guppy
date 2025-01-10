using Guppy.Core.Network.Common.Serialization;

namespace Guppy.Core.Network.Common.Services
{
    public interface INetSerializerService : IEnumerable<INetSerializer>
    {
        INetSerializer<T> Get<T>()
            where T : notnull;

        INetSerializer Get(Type type);

        INetSerializer Get(INetId id);
    }
}