using Autofac;
using Guppy.Common;
using Guppy.Common.Implementations;
using Guppy.Common.Providers;

namespace Guppy.Providers
{
    internal sealed class FilteredProvider : IFilteredProvider
    {
        private readonly IStateProvider _state;
        private readonly IComponentContext _context;
        private readonly IServiceFilterProvider _filters;

        public FilteredProvider(IComponentContext context, IServiceFilterProvider filters, IStateProvider state)
        {
            _state = state;
            _context = context;
            _filters = filters;
        }

        public IFiltered<T> Get<T>()
            where T : class
        {
            return new Filtered<T>(
                _state,
                _filters, 
                _context.Resolve<Lazy<IEnumerable<T>>>());
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
                _context.Resolve<Lazy<IEnumerable<T>>>());
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
