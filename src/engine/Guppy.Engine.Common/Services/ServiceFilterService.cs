using Autofac;

namespace Guppy.Engine.Common.Services
{
    internal sealed class ServiceFilterService : IServiceFilterService
    {
        private IServiceFilter[] _filters;
        private IDictionary<Type, IServiceFilter[]> _typeFilters;

        public ServiceFilterService(
            IEnumerable<IServiceFilter> filters)
        {
            _filters = filters.ToArray();
            _typeFilters = new Dictionary<Type, IServiceFilter[]>();
        }

        public bool Filter(ILifetimeScope scope, object service)
        {
            var type = service.GetType();
            if (!_typeFilters.TryGetValue(type, out var filters))
            {
                filters = _filters.Where(f => f.AppliesTo(type)).ToArray();
                _typeFilters.Add(type, filters);
            }

            foreach (var filter in filters)
            {
                if (!filter.Invoke(scope))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
