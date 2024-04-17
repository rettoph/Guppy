using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    internal sealed class FilteredService : IFilteredService
    {
        private readonly ILifetimeScope _scope;
        private readonly IComponentContext _context;
        private readonly IServiceFilterService _filters;

        public FilteredService(IComponentContext context, IServiceFilterService filters, ILifetimeScope scope)
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
    }
}
