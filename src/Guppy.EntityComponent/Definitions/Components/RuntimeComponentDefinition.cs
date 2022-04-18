using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Definitions.Components
{
    internal sealed class RuntimeComponentDefinition : ComponentDefinition
    {
        public override Type EntityType { get; }

        public override Type ComponentType { get; }

        public override Func<IServiceProvider, IEntity, IComponent> BuildComponent { get; }

        public RuntimeComponentDefinition(Type entityType, Type componentType, Func<IServiceProvider, IEntity, IComponent> factory)
        {
            this.EntityType = entityType;
            this.ComponentType = componentType;
            this.BuildComponent = factory;
        }
    }
}
