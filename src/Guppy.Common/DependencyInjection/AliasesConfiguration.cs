using Guppy.Common.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    public class AliasesConfiguration : IEnumerable<AliasConfiguration>
    {
        private readonly IServiceConfiguration _service;
        private readonly Dictionary<Type, AliasConfiguration> _aliases;

        internal AliasesConfiguration(IServiceConfiguration service)
        {
            _service = service;
            _aliases = new Dictionary<Type, AliasConfiguration>();
        }

        public bool Contains(Type type)
        {
            return _aliases.ContainsKey(type);
        }

        public void Add(AliasConfiguration alias)
        {
            ThrowIf.Type.IsNotAssignableFrom(alias.Type, _service.Type);

            _aliases.Add(alias.Type, alias);
        }

        public AliasConfiguration Get(Type type)
        {
            if(!_aliases.TryGetValue(type, out AliasConfiguration? configuration))
            {
                configuration = new AliasConfiguration(type);
                this.Add(configuration);
            }

            return configuration;
        }

        public IEnumerable<ServiceDescriptor> GetDescriptors()
        {
            return _aliases.Values.Select(a => a.GetDescriptor(_service));
        }

        IEnumerator<AliasConfiguration> IEnumerable<AliasConfiguration>.GetEnumerator()
        {
            return _aliases.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _aliases.Values.GetEnumerator();
        }
    }
}
