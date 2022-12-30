﻿using Guppy.Common.DependencyInjection;
using Guppy.Common.DependencyInjection.Interfaces;
using System.Diagnostics;

namespace Guppy.Common.Providers
{
    internal sealed class AliasProvider : IAliasProvider
    {
        private class FilterableAlias
        {
            public readonly IServiceConfiguration Service;
            public readonly AliasConfiguration Alias;
            public readonly IServiceFilter[] Filters;

            public FilterableAlias(IServiceConfiguration service, AliasConfiguration alias, IServiceFilter[] filters)
            {
                this.Service = service;
                this.Alias = alias;
                this.Filters = filters;
            }

            public bool Filter(IServiceProvider provider)
            {
                foreach(IServiceFilter filter in this.Filters)
                {
                    if(!filter.Invoke(provider, this.Service))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private readonly Dictionary<Type, FilterableAlias[]> _aliases;

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

            _aliases = new Dictionary<Type, FilterableAlias[]>(keys.Length);

            var filterableAliases = new List<FilterableAlias>();
            foreach (Type alias in keys)
            {
                foreach(IServiceConfiguration service in services)
                {
                    if(service.Aliases.Contains(alias))
                    {
                        filterableAliases.Add(new FilterableAlias(
                            service: service,
                            alias: service.Aliases.Get(alias),
                            filters: filters.Where(x => x.AppliesTo(service)).ToArray()));
                    }
                }

                _aliases.Add(alias, filterableAliases.OrderBy(x => x.Alias.Order).ToArray());
                filterableAliases.Clear();
            }
        }

        public IEnumerable<Type> GetServiceTypes(Type alias, IServiceProvider provider)
        {
            if(_aliases.TryGetValue(alias, out FilterableAlias[]? implementations))
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
