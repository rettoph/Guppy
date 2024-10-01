using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    internal sealed class FilteredService(IComponentContext context, IServiceFilterService filters, ILifetimeScope scope) : IFilteredService
    {
        private readonly ILifetimeScope _scope = scope;
        private readonly IComponentContext _context = context;
        private readonly IServiceFilterService _filters = filters;

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
