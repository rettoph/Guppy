namespace Guppy.Core.Network.Services
{
    public interface INetSerializerService : IEnumerable<INetSerializer>
    {
        INetSerializer<T> Get<T>()
            where T : notnull;

        INetSerializer Get(Type type);

        INetSerializer Get(INetId id);
    }
}
