using Guppy.EntityComponent.Definitions;
using Guppy.Network.Components;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.Setups
{
    internal sealed class NetTargetSetupDefinition : SetupDefinition<INetTarget>
    {
        private NetIdProvider _ids;

        public override int Order => SetupConstants.NetTargetSetupOrder;

        public NetTargetSetupDefinition(NetIdProvider ids)
        {
            _ids = ids;
        }

        protected override bool TryInitialize(INetTarget entity)
        {
            var messenger = entity.Components.Get<NetMessenger>();

            if(messenger.State != NetState.Stopped)
            {
                return true;
            }

            messenger.Start(_ids.Reserve());

            return true;
        }

        protected override bool TryUninitialize(INetTarget entity)
        {
            var messenger = entity.Components.Get<NetMessenger>();

            messenger.Stop();

            return true;
        }
    }
}
