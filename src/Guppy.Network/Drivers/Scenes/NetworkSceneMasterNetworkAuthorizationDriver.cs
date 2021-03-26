using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Lists.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Drivers.Scenes
{
    internal sealed class NetworkSceneMasterNetworkAuthorizationDriver : MasterNetworkAuthorizationDriver<NetworkScene>
    {
        #region Private Fields
        private EntityList _entities;
        private ServiceList<NetworkEntity> _networkEntities;

        private Queue<NetworkEntity> _creates;
        #endregion

        #region Lifecycle Methods
        protected override void InitializeRemote(NetworkScene driven, ServiceProvider provider)
        {
            base.InitializeRemote(driven, provider);

            provider.Service(out _entities);
            provider.Service(out _networkEntities);
        }

        protected override void ReleaseRemote(NetworkScene driven)
        {
            base.ReleaseRemote(driven);

            _networkEntities.Clear();

            _networkEntities = null;
            _entities = null;
        }
        #endregion

        #region SendMessage Methods
        private void SendCreateMessage(NetworkEntity entity)
        {

        }

        private void SendUpdateMessage(NetworkEntity entity)
        {

        }

        private void SendRemoveMessage(NetworkEntity entity)
        {

        }
        #endregion
    }
}
