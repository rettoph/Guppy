using Guppy.Attributes;
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

        public Type[] Filters { get; }

        public RuntimeSystemDefinition(Type type, int order, Type[] filters)
        {
            this.Order = order;
            this.Type = type;
            this.Filters = filters.Concat(WithFilterAttribute.GetTypes(type)).ToArray();
        }
    }
}
