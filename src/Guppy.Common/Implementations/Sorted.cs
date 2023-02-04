using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal sealed class Sorted<T> : ISorted<T>
        where T : class
    {
        private IList<T> _items;

        public Sorted(IFiltered<T> filtered)
        {
            _items = filtered.Instances.Sort().ToList();
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
