using Guppy.Common;
using Guppy.Common.Providers;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider _provider;
        private readonly IAliasProvider _aliases;

        private IEnumerable<T>? _unfiltered;
        private IEnumerable<T>? _items;
        private T? _item;

        public IEnumerable<T> Unfiltered => _unfiltered ??= _provider.GetRequiredService<IEnumerable<T>>();
        
        public IEnumerable<T> Instances => _items ??= this.Unfiltered.Concat(_aliases.GetServices<T>(_provider)).ToArray();

        public T? Instance => _item ??= _aliases.GetService<T>(_provider) ?? this.Unfiltered.LastOrDefault();

        public Filtered(
            IServiceProvider provider,
            IAliasProvider aliases)
        {
            _provider = provider;
            _aliases = aliases;
        }
    }
}
