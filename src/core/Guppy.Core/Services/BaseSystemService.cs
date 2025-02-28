using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Services
{
    public abstract class BaseSystemService<TSystem, TSystemProvider>(
        Lazy<IEnumerable<TSystem>> systems,
        Lazy<IEnumerable<TSystemProvider>> providers
    ) : ISystemService<TSystem>
        where TSystem : ISystem
        where TSystemProvider : ISystemProvider<TSystem>
    {

        private readonly Lazy<TSystem[]> _systems = new(() =>
        {
            return [.. systems.Value, .. providers.Value.SelectMany(x => x.GetSystems())];
        });

        public IEnumerable<TSystem> GetAll()
        {
            return this._systems.Value;
        }
    }
}
