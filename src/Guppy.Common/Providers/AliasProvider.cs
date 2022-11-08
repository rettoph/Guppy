using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal sealed class AliasProvider : IAliasProvider
    {
        [DebuggerDisplay("Type: {Type.Name}, Aliase: {AliasType.Name}, Filters: {Filters.Length}")]
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

            _aliases = new Dictionary<Type, FilterableImplementation[]>(keys.Length);

            var filteredAliases = new List<FilterableImplementation>();
            foreach (Type key in keys)
            {
                foreach(Alias alias in aliases)
                {
                    if(alias.Type == key)
                    {
                        filteredAliases.Add(new FilterableImplementation(
                            type: alias.ImplementationType,
                            aliasType: key,
                            filters: filters.Where(x => x.AppliesTo(alias.ImplementationType)).ToArray()));
                    }
                }

                _aliases.Add(key, filteredAliases.ToArray());
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
