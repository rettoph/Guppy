using Guppy.Core.Common.Systems;

namespace Guppy.Core.Common.Providers
{
    public interface ISystemProvider<TSystem>
        where TSystem : ISystem
    {
        IEnumerable<TSystem> GetSystems();
    }
}
