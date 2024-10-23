using Guppy.Core.Resources.Common.Interfaces;

namespace Guppy.Core.Resources.Common.Internals
{
    internal class RuntimeResourceValue<T>(Resource<T> resource, T value) : IRuntimeResourceValue
        where T : notnull
    {
        public Resource<T> Resource { get; } = resource;

        public T Value { get; } = value;

        public void AddToPack(IResourcePack resourcePack)
        {
            resourcePack.Add(this.Resource, this.Value);
        }
    }
}
