using Guppy.Common.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal sealed class FilteredProvider : IFilteredProvider
    {
        private readonly IServiceProvider _provider;
        private readonly IAliasProvider _aliases;

        public FilteredProvider(IServiceProvider provider, IAliasProvider aliases)
        {
            _provider = provider;
            _aliases = aliases;
        }

        public IFiltered<T> Get<T>(object? configuration)
            where T : class
        {
            return new Filtered<T>(_provider, _aliases, configuration);
        }

        public T? Instance<T>(object? configuration = null)
            where T : class
        {
            return this.Get<T>(configuration).Instance;
        }

        public IEnumerable<T> Instances<T>(object? configuration = null)
            where T : class
        {
            return this.Get<T>(configuration).Instances;
        }
    }
}
