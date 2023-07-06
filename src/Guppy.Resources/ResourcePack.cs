using Guppy.Resources.Constants;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Resources
{
    public sealed class ResourcePack
    {
        public readonly Guid Id;
        public string? Name { get; set; }

        private Dictionary<Resource, Dictionary<string, object>> _resources;

        public ResourcePack(Guid id)
        {
            this.Id = id;

            _resources = new Dictionary<Resource, Dictionary<string, object>>();
        }

        public void Add<T>(Resource<T> resource, string localization, T value)
            where T : notnull
        {
            if(!_resources.TryGetValue(resource, out var values))
            {
                _resources[resource] = values = new();
            }

            values.Add(localization, value);
        }

        public void Add<T>(Resource<T> resource, T value)
            where T : notnull
        {
            this.Add(resource, Localization.Default, value);
        }

        public bool TryGet<T>(Resource<T> resource, string localization, [MaybeNullWhen(false)] out T value)
            where T : notnull
        {
            if (!_resources.TryGetValue(resource, out var values))
            {
                value = default!;
                return false;
            }

            if(values.TryGetValue(localization, out object? cached))
            {
                value = (T)cached;
                return true;
            }

            value = default!;
            return false;
        }
    }
}
