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
        private readonly IStateProvider _state;
        private readonly IServiceProvider _provider;
        private readonly IServiceFilterProvider _filters;

        public FilteredProvider(IServiceProvider provider, IServiceFilterProvider filters, IStateProvider state)
        {
            _state = state;
            _provider = provider;
            _filters = filters;
        }

        public IFiltered<T> Get<T>()
            where T : class
        {
            return new Filtered<T>(
                _state,
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

        public IFiltered<T> Get<T>(params IState[] states) where T : class
        {
            return new Filtered<T>(
                _state.Custom(states),
                _filters,
                _provider.GetRequiredService<Lazy<IEnumerable<T>>>());
        }

        public T? Instance<T>(params IState[] states) where T : class
        {
            return this.Get<T>(states).Instance;
        }

        public IEnumerable<T> Instances<T>(params IState[] states) where T : class
        {
            return this.Get<T>(states).Instances;
        }
    }
}
