using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    internal sealed class RuntimeSystemDefinition : ISystemDefinition
    {
        private Func<IServiceProvider, bool> _filter;

        public int Order { get; }

        public Type Type { get; }

        public RuntimeSystemDefinition(Type type, Func<IServiceProvider, bool> filter, int order)
        {
            _filter = filter;

            this. Order = order;
            this. Type = type;
        }

        public bool Filter(IServiceProvider provider)
        {
            return _filter(provider);
        }
    }
}
