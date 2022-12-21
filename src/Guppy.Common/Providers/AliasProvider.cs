using Guppy.Common.DependencyInjection;
using Guppy.Common.DependencyInjection.Interfaces;
using System.Diagnostics;

namespace Guppy.Common.Providers
{
    internal sealed class AliasProvider : IAliasProvider
    {
        [DebuggerDisplay("Type: {Type.Name}, Aliase: {AliasType.Name}, Filters: {Filters.Length}")]
        private class FilterableImplementation
        {
            public readonly IServiceConfiguration Service;
            public readonly AliasConfiguration Alias;
            public readonly IFilter[] Filters;

            public FilterableImplementation(IServiceConfiguration service, AliasConfiguration alias, IFilter[] filters)
            {
                this.Service = service;
                this.Alias = alias;
                this.Filters = filters;
            }

            public bool Filter(IServiceProvider provider)
            {
                foreach(IFilter filter in this.Filters)
                {
                    if(!filter.Invoke(provider, this.Service.Type))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private readonly Dictionary<Type, FilterableImplementation[]> _aliases;

        public AliasProvider(IEnumerable<IServiceConfiguration> services, IEnumerable<IFilter> filters)
        {
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
                            filters: filters.Where(x => x.AppliesTo(service.Type)).ToArray()));
                    }
                }

                _aliases.Add(alias, filteredAliases.ToArray());
                filteredAliases.Clear();
            }
        }

        public IEnumerable<Type> GetServiceTypes(Type alias, IServiceProvider provider)
        {
            if(_aliases.TryGetValue(alias, out FilterableImplementation[]? implementations))
            {
                foreach(var implementation in implementations)
                {
                    if(implementation.Filter(provider))
                    {
                        yield return implementation.Service.ServiceType;
                    }
                }
            }
        }
    }
}
