using Guppy.EntityComponent.Definitions;
using Guppy.Network.Providers;
using Guppy.Network.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.Setups
{
    internal sealed class NetTargetSetupDefinition : SetupDefinition<INetTarget>
    {
        private INetTargetService _targets;
        private NetScope _scope;
        private NetIdProvider _ids;

        public override int Order => -1;

        public NetTargetSetupDefinition()
        {
            _targets = null!;
            _scope = null!;
            _ids = null!;
        }

        protected override void Initialize(IServiceProvider provider)
        {
            base.Initialize(provider);

            _targets = provider.GetRequiredService<INetTargetService>();
            _scope = provider.GetRequiredService<NetScope>();
            _ids = new NetIdProvider();
        }

        protected override bool TryCreate(INetTarget entity)
        {
            if(entity.NetId == default)
            {
                entity.NetId = _ids.Reserve();
            }
            
            entity.Messages = new MessageService(entity, _scope);

            return _targets.TryAdd(entity);
        }

        protected override bool TryDestroy(INetTarget entity)
        {
            _ids.Release(entity.NetId);
            entity.Messages.Dispose();

            return _targets.TryRemove(entity);
        }
    }
}
