using Guppy.EntityComponent;
using Guppy.EntityComponent.Definitions;
using Guppy.Gaming.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Definitions.Setups
{
    internal sealed class LayerSetupDefinition : SetupDefinition<ILayer>
    {
        private ILayerService _layers;

        public LayerSetupDefinition(ILayerService layers)
        {
            _layers = layers;
        }

        protected override bool TryCreate(ILayer entity)
        {
            _layers.Add(entity);

            return true;
        }

        protected override bool TryDestroy(ILayer entity)
        {
            _layers.Remove(entity);

            return true;
        }
    }
}
