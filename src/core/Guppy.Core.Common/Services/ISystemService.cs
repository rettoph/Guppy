namespace Guppy.Core.Common.Services
{
    public interface ISystemService<TSystem> : IEnumerable<TSystem>
        where TSystem : ISystem
    {
        IEnumerable<T> GetAll<T>();
    }
}
