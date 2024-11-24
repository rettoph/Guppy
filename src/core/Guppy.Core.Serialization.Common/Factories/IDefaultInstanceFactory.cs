namespace Guppy.Core.Serialization.Common.Factories
{
    public interface IDefaultInstanceFactory
    {
        bool CanConstructType(Type type);

        object BuildInstance(Type type);
    }
}
