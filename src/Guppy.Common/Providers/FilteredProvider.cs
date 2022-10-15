using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal sealed class FilteredProvider<T>
        where T : class
    {
        private IList<IServiceTypeFilter<T>> _typeFilters;
        private Type[]? _implementationTypes;

        public FilteredProvider(IEnumerable<IServiceTypeFilter<T>> typeFilters)
        {
            _typeFilters = typeFilters.OrderBy(f => f.Order).ToList();
        }

        public Type[] GetImplementationTypes(IServiceProvider provider)
        {
            if(_implementationTypes is not null)
            {
                return _implementationTypes;
            }

            List<Type> implementationTypes = new List<Type>();

            foreach (IServiceTypeFilter<T> filter in _typeFilters)
            {
                if (filter.Invoke(provider))
                {
                    implementationTypes.Add(filter.ImplementationType);
                }
            }

            _implementationTypes = implementationTypes.ToArray();

            return _implementationTypes;
        }
    }
}
