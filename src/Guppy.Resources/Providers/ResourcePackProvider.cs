using Guppy.Resources.Loaders;
using Guppy.Resources.Serialization.Json;

namespace Guppy.Resources.Providers
{
    internal sealed class ResourcePackProvider : IResourcePackProvider
    {
        private IDictionary<Guid, ResourcePack> _packs;

        public ResourcePackProvider(
            IEnumerable<ResourcePack> packs,
            IEnumerable<IPackLoader> loaders)
        {
            _packs = packs.ToDictionary(x => x.Id, x => x);

            foreach(var loader in loaders)
            {
                loader.Load(this);
            }
        }

        public void Configure(Guid id, Action<ResourcePack> configurator)
        {
            if(!_packs.TryGetValue(id, out var pack))
            {
                _packs[id] = pack = new ResourcePack(id);
            }

            configurator(pack);
        }

        public IEnumerable<ResourcePack> GetAll()
        {
            return _packs.Values;
        }

        public ResourcePack GetById(Guid id)
        {
            return _packs[id];
        }
    }
}
