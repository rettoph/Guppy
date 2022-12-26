using Guppy.Common.DependencyInjection;
using Guppy.Common.DependencyInjection.Interfaces;
using System.Diagnostics;

namespace Guppy.Common.Providers
{
    internal sealed class AliasProvider : IAliasProvider
    {
        [DebuggerDisplay("Service: {Service.Type.Name}, Alias: {AliasType.Name}, Filters: {Filters.Length}")]
        private class FilterableImplementation
        {
            public readonly IServiceConfiguration Service;
            public readonly AliasConfiguration Alias;
            public readonly IServiceFilter[] Filters;

            public FilterableImplementation(IServiceConfiguration service, AliasConfiguration alias, IServiceFilter[] filters)
            {
                this.Service = service;
                this.Alias = alias;
                this.Filters = filters;
            }

            public bool Filter(IServiceProvider provider, object? configuration)
            {
                foreach(IServiceFilter filter in this.Filters)
                {
                    if(!filter.Invoke(provider, this.Service, configuration))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private readonly Dictionary<Type, FilterableImplementation[]> _aliases;

        public AliasProvider(
            IServiceProvider provider, 
            IEnumerable<IServiceConfiguration> services,
            IEnumerable<IServiceFilter> filters)
        {
            // Initialize all filters
            foreach(IServiceFilter filter in filters)
            {
                filter.Initialize(provider);
            }

            // Load distinct alias types
            var keys = services.SelectMany(x => x.Aliases)
                .Select(x => x.Type)
                .Distinct()
                .ToArray();

            _aliases = new Dictionary<Type, FilterableImplementation[]>(keys.Length);

            var filteredAliases = new List<FilterableImplementation>();
            foreach (Type alias in keys)
            {
                foreach(IServiceConfiguration service in services)
                {
                    if(service.Aliases.Contains(alias))
                    {
                        filteredAliases.Add(new FilterableImplementation(
                            service: service,
                            alias: service.Aliases.Get(alias),
                            filters: filters.Where(x => x.AppliesTo(service)).ToArray()));
                    }
                }

                _aliases.Add(alias, filteredAliases.OrderBy(x => x.Alias.Order).ToArray());
                filteredAliases.Clear();
            }
        }

        public IEnumerable<Type> GetServiceTypes(Type alias, IServiceProvider provider, object? configuration)
        {
            if(_aliases.TryGetValue(alias, out FilterableImplementation[]? implementations))
            {
                foreach(var implementation in implementations)
                {
                    if(implementation.Filter(provider, configuration))
                    {
                        yield return implementation.Service.ServiceType;
                    }
                }
            }
        }
    }
}
