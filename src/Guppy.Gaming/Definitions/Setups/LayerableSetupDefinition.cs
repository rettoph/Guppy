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
    internal sealed class LayerableSetupDefinition : SetupDefinition<ILayerable>
    {
        private ILayerableService _layerables;

        public LayerableSetupDefinition(ILayerableService layerables)
        {
            _layerables = layerables;
        }

        protected override bool TryInitialize(ILayerable entity)
        {
            _layerables.Add(entity);

            return true;
        }

        protected override bool TryUninitialize(ILayerable entity)
        {
            _layerables.Remove(entity);

            return true;
        }
    }
}
