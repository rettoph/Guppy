using Guppy.Resources.Constants;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Guppy.Resources
{
    public sealed class ResourcePack
    {
        public readonly Guid Id;
        public string? Name { get; set; }

        private Dictionary<Resource, Dictionary<string, List<object>>> _resources;

        public ResourcePack(Guid id)
        {
            this.Id = id;

            _resources = new Dictionary<Resource, Dictionary<string, List<object>>>();
        }

        public void Add<T>(Resource<T> resource, string localization, T value)
            where T : notnull
        {
            if(!_resources.TryGetValue(resource, out var values))
            {
                _resources[resource] = values = new();
            }

            ref List<object>? localized = ref CollectionsMarshal.GetValueRefOrAddDefault(values, localization, out bool exists);
            localized ??= new List<object>();

            localized.Add(value);
        }

        public void Add<T>(Resource<T> resource, T value)
            where T : notnull
        {
            this.Add(resource, Localization.Default, value);
        }

        public bool TryGet<T>(Resource<T> resource, string localization, out IEnumerable<T> value)
            where T : notnull
        {
            if (!_resources.TryGetValue(resource, out var values))
            {
                value = Enumerable.Empty<T>();
                return false;
            }

            if(values.TryGetValue(localization, out List<object>? cached))
            {
                value = cached.OfType<T>();
                return true;
            }

            value = Enumerable.Empty<T>();
            return false;
        }
    }
}
