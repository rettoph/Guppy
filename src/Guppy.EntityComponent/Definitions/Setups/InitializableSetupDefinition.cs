using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Definitions.Setups
{
    internal sealed class InitializableSetupDefinition : SetupDefinition<IInitializable>
    {
        public override int Order => Constants.SetupOrders.InitializableSetupOrder;

        protected override bool TryInitialize(IInitializable entity)
        {
            entity.Initialize();

            return true;
        }

        protected override bool TryUninitialize(IInitializable entity)
        {
            entity.Uninitialize();

            return true;
        }
    }
}
