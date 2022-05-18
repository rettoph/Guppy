using Guppy.EntityComponent.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Definitions.Setups
{
    internal sealed class ComponentSetupDefinition : SetupDefinition
    {
        private readonly IComponentProvider _components;

        public override int Order => Constants.SetupOrders.ComponentSetupOrder;

        public ComponentSetupDefinition(IComponentProvider components)
        {
            _components = components;
        }

        protected override bool TryCreate(IEntity entity)
        {
            entity.Components = _components.Create(entity);

            return true;
        }

        protected override bool TryDestroy(IEntity entity)
        {
            entity.Components.Dispose();

            return true;
        }
    }
}
