using Guppy.EntityComponent;
using Guppy.EntityComponent.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    internal sealed class ComponentSetup : Setup
    {
        private IServiceProvider _provider;
        private IComponentProvider _components;

        public ComponentSetup(
            IServiceProvider provider,
            IComponentProvider components)
        {
            _provider = provider;
            _components = components;
        }

        public override bool TryCreate(IEntity entity)
        {
            entity.Components = _components.Create(entity, _provider);

            return true;
        }

        public override bool TryDestroy(IEntity entity)
        {
            entity.Components.Dispose();

            return true;
        }
    }
}
