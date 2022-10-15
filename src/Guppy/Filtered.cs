using Guppy.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    internal sealed class Filtered<T> : IFiltered<T>
        where T : class
    {
        private IList<T> _items;

        public IEnumerable<T> Items => _items;

        public Filtered(
            IServiceProvider provider,
            IEnumerable<T> items,
            IEnumerable<IServiceTypeFilter<T>> filters)
        {
            _items = new List<T>(items);

            foreach(var filter in filters)
            {
                if(filter.Invoke(provider) && provider.GetService(filter.ImplementationType) is T casted)
                {
                    _items.Add(casted);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
