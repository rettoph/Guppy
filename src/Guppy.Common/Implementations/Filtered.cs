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
        private readonly Lazy<IList<T>> _items;
        private readonly Lazy<T?> _instance;

        public IEnumerable<T> Items => _items.Value;

        public T? Instance => _instance.Value;

        public Filtered(
            IAliasProvider aliases,
            IServiceProvider provider,
            IEnumerable<T> items)
        {
            _items = new Lazy<IList<T>>(() =>
            {
                var list = new List<T>(items.Concat(aliases.GetServices<T>(provider)));

                return list;
            });

            _instance = new Lazy<T?>(() =>
            {
                return aliases.GetService<T>(provider) ?? items.LastOrDefault();
            });
        }
    }
}
