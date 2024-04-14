namespace Guppy.Engine.Common
{
    public interface IServiceFactory<T>
    {
        T Get();
        T Get(string name);
    }
}
