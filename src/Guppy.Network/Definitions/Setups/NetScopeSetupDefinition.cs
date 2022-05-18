using Guppy.EntityComponent.Definitions;
using Guppy.Network.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.Setups
{
    internal sealed class NetScopeSetupDefinition : SetupDefinition<NetScope>
    {
        private INetScopeProvider _scopes;

        public NetScopeSetupDefinition(INetScopeProvider scopes)
        {
            _scopes = scopes;
        }

        protected override bool TryCreate(NetScope entity)
        {
            return _scopes.TryAdd(entity);
        }

        protected override bool TryDestroy(NetScope entity)
        {
            return _scopes.TryRemove(entity);
        }
    }
}
