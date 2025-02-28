using Guppy.Core.Common.Systems;

namespace Guppy.Core.Common.Providers
{
    public interface ISystemProvider<TSystem>
        where TSystem : ISystem
    {
        public IEnumerable<TSystem> GetSystems();
    }
}
