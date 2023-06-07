using Guppy.Common.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal sealed class ServiceFilterProvider : IServiceFilterProvider
    {
        private IServiceFilter[] _filters;
        private IDictionary<Type, IServiceFilter[]> _typeFilters;

        public ServiceFilterProvider(
            IStateProvider state,
            IEnumerable<IServiceFilter> filters)
        {
            _filters = filters.ToArray();
            _typeFilters = new Dictionary<Type, IServiceFilter[]>();
        }

        public bool Filter(IStateProvider state, object service)
        {
            var type = service.GetType();
            if (!_typeFilters.TryGetValue(type, out var filters))
            {
                filters = _filters.Where(f => f.AppliesTo(type)).ToArray();
                _typeFilters.Add(type, filters);
            }

            foreach(var filter in filters)
            {
                if(!filter.Invoke(state))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
