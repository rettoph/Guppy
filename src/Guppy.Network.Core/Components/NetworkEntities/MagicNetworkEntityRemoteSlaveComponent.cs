using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Threading.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components.NetworkEntities
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Slave)]
    internal sealed class MagicNetworkEntityRemoteSlaveComponent : Component<IMagicNetworkEntity>,
        IDataProcessor<CreateNetworkEntityMessage>,
        IDataProcessor<DisposeNetworkEntityMessage>
    {
        #region Private Fields
        private ILogger _logger;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _logger);

            this.Entity.Messages.RegisterProcessor<CreateNetworkEntityMessage>(this);
            this.Entity.Messages.RegisterProcessor<DisposeNetworkEntityMessage>(this);
        }

        protected override void PostUninitialize()
        {
            base.PostUninitialize();

            this.Entity.Messages.DeregisterProcessor<CreateNetworkEntityMessage>(this);
            this.Entity.Messages.DeregisterProcessor<DisposeNetworkEntityMessage>(this);
        }
        #endregion

        #region Network Methods
        bool IDataProcessor<CreateNetworkEntityMessage>.Process(CreateNetworkEntityMessage data)
        {
            _logger.Verbose("Processed {message} for {entity} ('{id}')", nameof(CreateNetworkEntityMessage), this.Entity.GetType().Name, this.Entity.NetworkId);
            return true;
        }

        bool IDataProcessor<DisposeNetworkEntityMessage>.Process(DisposeNetworkEntityMessage data)
        {
            _logger.Verbose("Processed {message} for {entity} ('{id}')", nameof(DisposeNetworkEntityMessage), this.Entity.GetType().Name, this.Entity.NetworkId);
            return true;
        }
        #endregion
    }
}
