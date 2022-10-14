using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    internal sealed class Filtered<T>
        where T : class
    {
        private IList<IServiceTypeFilter<T>> _typeFilters;

        public Filtered(IEnumerable<IServiceTypeFilter<T>> typeFilters)
        {
            _typeFilters = typeFilters.OrderBy(f => f.Order).ToList();
        }

        public Type? GetImplementationType(IServiceProvider provider)
        {
            foreach (IServiceTypeFilter<T> filter in _typeFilters)
            {
                if (filter.Invoke(provider))
                {
                    return filter.ImplementationType;
                }
            }

            return default;
        }
    }
}
