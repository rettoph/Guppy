using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;
using System.Runtime.InteropServices;

namespace Guppy.Core.Services
{
    internal sealed class ServiceFilterService(IEnumerable<IServiceFilter> filters) : IServiceFilterService
    {
        private readonly IServiceFilter[] _filters = filters.ToArray();
        private readonly Dictionary<Type, IServiceFilter[]> _typeFilters = new Dictionary<Type, IServiceFilter[]>();

        public bool Filter(ILifetimeScope scope, object service)
        {
            foreach (IServiceFilter filter in this.GetFilters(service.GetType()))
            {
                if (!filter.Invoke(scope))
                {
                    return false;
                }
            }

            return true;
        }

        public IServiceFilter[] GetFilters(Type type)
        {
            ref IServiceFilter[]? filters = ref CollectionsMarshal.GetValueRefOrAddDefault(_typeFilters, type, out bool exists);
            if (exists)
            {
                return filters!;
            }

            filters = _filters.Where(f => f.AppliesTo(type)).ToArray();
            return filters;
        }
    }
}
