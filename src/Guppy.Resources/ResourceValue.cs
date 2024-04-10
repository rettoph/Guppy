using Guppy.Common;
using Guppy.Resources.Providers;
using System.Runtime.InteropServices;

namespace Guppy.Resources
{
    internal interface IResourceValue
    {
        void ForceUpdate(ResourceProvider resources);
    }

    public sealed class ResourceValue<T> : Ref<T>, IResourceValue, IDisposable
        where T : notnull
    {
        public readonly Resource<T> Resource;

        public ResourceValue(Resource<T> resource) : base(default!)
        {
            this.Resource = resource;
        }

        void IResourceValue.ForceUpdate(ResourceProvider resources)
        {
            this.ForceUpdate(resources);
        }

        internal void ForceUpdate(ResourceProvider resources)
        {
            this.Value = resources.GetPackValue(this.Resource);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private static readonly Dictionary<Resource<T>, ResourceValue<T>> _cache = new Dictionary<Resource<T>, ResourceValue<T>>();
        internal static ResourceValue<T> Get(Resource<T> resource, out bool exists)
        {
            ref ResourceValue<T>? value = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, resource, out exists);
            if (exists)
            {
                return value!;
            }

            value = new ResourceValue<T>(resource);
            ResourceValueHelper.Add(value);
            return value;
        }

        public static void Clear()
        {
            foreach (ResourceValue<T> value in _cache.Values)
            {
                value.Dispose();
            }

            _cache.Clear();
        }
    }

    internal static class ResourceValueHelper
    {
        private static List<IResourceValue> _values = new List<IResourceValue>();

        public static IEnumerable<IResourceValue> GetAll()
        {
            return _values;
        }

        public static void Add(IResourceValue value)
        {
            _values.Add(value);
        }

        public static void Remove(IResourceValue value)
        {
            _values.Remove(value);
        }
    }
}
