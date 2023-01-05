using Guppy.Common.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal sealed class FilteredProvider : IFilteredProvider
    {
        private readonly IServiceProvider _provider;
        private readonly IFilterProvider _filters;

        public FilteredProvider(IServiceProvider provider, IFilterProvider filters)
        {
            _provider = provider;
            _filters = filters;
        }

        public IFiltered<T> Get<T>()
            where T : class
        {
            return new Filtered<T>(
                _filters, 
                _provider.GetRequiredService<Lazy<IEnumerable<T>>>());
        }

        public T? Instance<T>()
            where T : class
        {
            return this.Get<T>().Instance;
        }

        public IEnumerable<T> Instances<T>()
            where T : class
        {
            return this.Get<T>().Instances;
        }
    }
}
