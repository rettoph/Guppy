using Guppy.Common.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal sealed class FilterProvider : IFilterProvider
    {
        private IServiceProvider _provider;
        private IServiceFilter[] _filters;
        private IDictionary<Type, IServiceFilter[]> _typeFilters;

        public FilterProvider(
            IServiceProvider provider,
            IEnumerable<IServiceFilter> filters)
        {
            _provider = provider;
            _filters = filters.ToArray();
            _typeFilters = new Dictionary<Type, IServiceFilter[]>();

            // Initialize all filters
            foreach (IServiceFilter filter in _filters)
            {
                filter.Initialize(provider);
            }
        }

        public bool Filter(object service)
        {
            var type = service.GetType();
            if (!_typeFilters.TryGetValue(type, out var filters))
            {
                filters = _filters.Where(f => f.AppliesTo(type)).ToArray();
                _typeFilters.Add(type, filters);
            }

            foreach(var filter in filters)
            {
                if(!filter.Invoke(_provider, service))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
