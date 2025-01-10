namespace Guppy.Core.Common.Services
{
    public interface ITypeService<T> : IEnumerable<Type>
    {
        IEnumerable<T> CreateInstances();
    }
}