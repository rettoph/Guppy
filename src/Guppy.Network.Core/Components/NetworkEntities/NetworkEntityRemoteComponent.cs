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
    internal sealed class NetworkEntityRemoteComponent : NetworkComponent<INetworkEntity>
    {
        #region Private Fields
        private NetworkEntityService _networkEntities;
        private NetworkIdProvider _idProvider;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitializeRemote(ServiceProvider provider, NetworkAuthorization networkAuthorization)
        {
            base.PreInitializeRemote(provider, networkAuthorization);

            provider.Service(out _networkEntities);

            if (networkAuthorization == NetworkAuthorization.Master)
            { // Define the network id automatically for master authorizations...
                provider.Service(out _idProvider);

                this.Entity.NetworkId = _idProvider.ClaimId();
            }
        }

        protected override void InitializeRemote(ServiceProvider provider, NetworkAuthorization networkAuthorization)
        {
            base.InitializeRemote(provider, networkAuthorization);

            _networkEntities.TryAdd(this.Entity);
        }

        protected override void ReleaseRemote(NetworkAuthorization networkAuthorization)
        {
            base.ReleaseRemote(networkAuthorization);

            _networkEntities.Remove(this.Entity);
        }

        protected override void PostReleaseRemote(NetworkAuthorization networkAuthorization)
        {
            base.PostReleaseRemote(networkAuthorization);

            if (networkAuthorization == NetworkAuthorization.Master)
            { // Define the network id automatically for master authorizations...
                _idProvider.SurrenderId(this.Entity.NetworkId);

                _idProvider = default;
            }

            _networkEntities = default;
        }
        #endregion
    }
}
