using Guppy.Core.Resources.Common.Interfaces;

namespace Guppy.Core.Resources.Common.Internals
{
    internal class RuntimeResource<T>(ResourceKey<T> key, T value, string localization) : IRuntimeResource
        where T : notnull
    {
        public ResourceKey<T> Key { get; } = key;

        public T Value { get; } = value;

        public string Localization { get; } = localization;

        public void AddToPack(IResourcePack resourcePack)
        {
            resourcePack.Add(this.Key, this.Localization, this.Value);
        }
    }
}
