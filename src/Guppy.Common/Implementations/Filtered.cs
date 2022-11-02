using Guppy.Common;
using Guppy.Common.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal sealed class Filtered<T> : IFiltered<T>
        where T : class
    {
        private IList<T> _items;

        public IEnumerable<T> Items => _items;

        public T Instance => _items[0];

        public Filtered(
            IAliasProvider aliases,
            IServiceProvider provider,
            IEnumerable<T> items)
        {
            _items = new List<T>(items.Concat(aliases.GetImplementations<T>(provider)));
        }
    }
}
