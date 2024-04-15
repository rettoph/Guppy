namespace Guppy.Core.Common
{
    public interface IServiceFactory<T>
    {
        T Get();
        T Get(string name);
    }
}
