using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    internal sealed class NetworkEntityRemoteMasterIdComponent : Component<INetworkEntity>
    {
        #region Private Fields
        private NetworkIdProvider _idProvider;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            // Define the network id automatically for master authorizations...
            provider.Service(out _idProvider);
            this.Entity.NetworkId = _idProvider.ClaimId();
        }

        protected override void PostUninitialize()
        {
            base.PostUninitialize();

            _idProvider.SurrenderId(this.Entity.NetworkId);
        }
        #endregion
    }
}
