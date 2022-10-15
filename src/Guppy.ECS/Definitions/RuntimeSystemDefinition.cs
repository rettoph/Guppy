using Guppy.Attributes;
using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    internal sealed class RuntimeSystemDefinition : ISystemDefinition
    {
        public int Order { get; }

        public Type Type { get; }

        public IFilter<ISystemDefinition>[] Filters { get; }

        public RuntimeSystemDefinition(Type type, int order, IFilter<ISystemDefinition>[] filters)
        {
            this.Order = order;
            this.Type = type;
            this.Filters = filters.Concat(FactoryAttribute.GetInstances<IFilter<ISystemDefinition>>(type)).ToArray();
        }
    }
}
