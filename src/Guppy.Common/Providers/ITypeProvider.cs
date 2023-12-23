namespace Guppy.Common.Providers
{
    public interface ITypeProvider<T> : IEnumerable<Type>
    {
        IEnumerable<T> CreateInstances();
    }
}
