using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    internal sealed class RuntimeComponentDefinition<T> : ComponentDefinition<T>
        where T : class
    {
        private Func<IServiceProvider, T> _factory;
        private IServiceProvider _provider;

        public override EntityTag[] Tags { get; }

        public RuntimeComponentDefinition(IServiceProvider provider, Func<IServiceProvider, T> factory, EntityTag[] tags)
        {
            _factory = factory;
            _provider = provider;

            this.Tags = tags;
        }

        public override T Factory()
        {
            return _factory(_provider);
        }
    }
}
