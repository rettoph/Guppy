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
