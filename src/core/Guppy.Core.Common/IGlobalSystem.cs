namespace Guppy.Core.Common
{
    public interface IGlobalSystem : ISystem
    {
    }

    public interface IGlobalSystem<T> : IGlobalSystem, ISystem<T>
    {
    }
}
