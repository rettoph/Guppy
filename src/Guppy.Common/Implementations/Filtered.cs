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
        private readonly object? _configuration;

        private IEnumerable<T>? _unfiltered;
        private IEnumerable<T>? _items;
        private T? _item;

        public IEnumerable<T> Unfiltered => _unfiltered ??= _provider.GetRequiredService<IEnumerable<T>>();
        
        public IEnumerable<T> Instances => _items ??= this.Unfiltered.Concat(_aliases.GetServices<T>(_provider, _configuration)).ToArray();

        public T? Instance => _item ??= _aliases.GetService<T>(_provider, _configuration) ?? this.Unfiltered.LastOrDefault();

        public Filtered(
            IServiceProvider provider,
            IAliasProvider aliases) : this(provider, aliases, null)
        {

        }
        public Filtered(
            IServiceProvider provider,
            IAliasProvider aliases,
            object? configuration)
        {
            _provider = provider;
            _aliases = aliases;
            _configuration = configuration;
        }
    }
}
