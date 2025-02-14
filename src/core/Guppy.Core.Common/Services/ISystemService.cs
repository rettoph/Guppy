using Guppy.Core.Common.Systems;

namespace Guppy.Core.Common.Services
{
    public interface ISystemService<TSystem>
        where TSystem : ISystem
    {
        IEnumerable<TSystem> GetAll();
        IEnumerable<T> GetAll<T>()
        {
            return this.GetAll().OfType<T>();
        }
    }
}
