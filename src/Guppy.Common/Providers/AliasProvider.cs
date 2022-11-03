using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal sealed class AliasProvider : IAliasProvider
    {
        private class ImplementationAliases
        {
            public readonly Type Type;
            public readonly Type[] AliasTypes;

            public ImplementationAliases(Type implementationType, Type[] aliasTypes)
            {
                this.Type = implementationType;
                this.AliasTypes = aliasTypes;
            }
        }

        private class FilterableImplementation
        {
            public readonly Type Type;
            public readonly Type AliasType;
            public readonly IFilter[] Filters;

            public FilterableImplementation(Type type, Type aliasType, IFilter[] filters)
            {
                this.Type = type;
                this.AliasType = aliasType;
                this.Filters = filters;
            }

            public bool Filter(IServiceProvider provider)
            {
                foreach(IFilter filter in this.Filters)
                {
                    if(!filter.Invoke(provider, this.Type))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private readonly Dictionary<Type, FilterableImplementation[]> _aliases;

        public AliasProvider(IEnumerable<Alias> aliases, IEnumerable<IFilter> filters)
        {
            // Load distinct alias types
            var keys = aliases.Select(x => x.Type).Distinct().ToArray();

            // Break all aliases by implementaion type and map the distinct alias types to each implementation
            var implementations = aliases.GroupBy(x => x.ImplementationType)
                .Select(g => new ImplementationAliases(g.Key, g.Select(x => x.Type).Distinct().ToArray()))
                .ToArray();

            _aliases = new Dictionary<Type, FilterableImplementation[]>(keys.Length);

            var filteredAliases = new List<FilterableImplementation>();
            foreach (Type alias in keys)
            {
                foreach(ImplementationAliases implementation in implementations)
                {
                    if(implementation.AliasTypes.Contains(alias))
                    {
                        filteredAliases.Add(new FilterableImplementation(
                            type: implementation.Type,
                            aliasType: alias,
                            filters: filters.Where(x => x.AppliesTo(implementation.Type)).ToArray()));
                    }
                }

                _aliases.Add(alias, filteredAliases.ToArray());
                filteredAliases.Clear();
            }
        }

        public IEnumerable<Type> GetImplementationTypes(Type alias, IServiceProvider provider)
        {
            if(_aliases.TryGetValue(alias, out FilterableImplementation[]? implementations))
            {
                foreach(var implementation in implementations)
                {
                    if(implementation.Filter(provider))
                    {
                        yield return implementation.Type;
                    }
                }
            }
        }
    }
}
