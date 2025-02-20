namespace Guppy.Core.Common
{
    public interface IGuppyContainer : IDisposable
    {
        T Resolve<T>()
            where T : class;

        object Resolve(Type type);
    }
}
