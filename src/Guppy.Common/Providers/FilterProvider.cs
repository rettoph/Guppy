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
        private IStateProvider _state;
        private IServiceFilter[] _filters;
        private IDictionary<Type, IServiceFilter[]> _typeFilters;

        public FilterProvider(
            IStateProvider state,
            IEnumerable<IServiceFilter> filters)
        {
            _state = state;
            _filters = filters.ToArray();
            _typeFilters = new Dictionary<Type, IServiceFilter[]>();
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
                if(!filter.Invoke(_state, service))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
