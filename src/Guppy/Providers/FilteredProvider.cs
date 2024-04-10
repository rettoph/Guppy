using Autofac;
using Guppy.Common;
using Guppy.Common.Services;

namespace Guppy.Providers
{
    internal sealed class FilteredProvider : IFilteredService
    {
        private readonly ILifetimeScope _scope;
        private readonly IComponentContext _context;
        private readonly IServiceFilterService _filters;

        public FilteredProvider(IComponentContext context, IServiceFilterService filters, ILifetimeScope scope)
        {
            _scope = scope;
            _context = context;
            _filters = filters;
        }

        public IFiltered<T> Get<T>()
            where T : class
        {
            return new Filtered<T>(
                _scope,
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
    }
}
