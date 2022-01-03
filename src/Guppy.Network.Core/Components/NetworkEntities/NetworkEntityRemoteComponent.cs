using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Services;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components.NetworkEntities
{
    [HostTypeRequired(HostType.Remote)]
    internal sealed class NetworkEntityRemoteComponent : Component<INetworkEntity>
    {
        #region Private Fields
        private NetworkEntityService _networkEntities;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _networkEntities);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _networkEntities.TryAdd(this.Entity);
        }

        protected override void Uninitialize()
        {
            base.Uninitialize();

            _networkEntities.Remove(this.Entity);
        }
        #endregion
    }
}
