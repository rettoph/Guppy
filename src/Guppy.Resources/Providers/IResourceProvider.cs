namespace Guppy.Resources.Providers
{
    public interface IResourceProvider : IGlobalComponent
    {
        bool Ready { get; }

        ResourceValue<T> Get<T>(Resource<T> resource)
            where T : notnull;

        IEnumerable<(Resource, T)> GetAll<T>()
            where T : notnull;

        IResourceProvider Set<T>(Resource<T> resource, T value)
            where T : notnull;
    }
}
