using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Network.Services;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.MessageProcessors
{
    internal sealed class CreateNetworkEntityMessageProcessor : Service, IMessageProcessor<CreateNetworkEntityMessage>
    {
        #region Private Fields
        private NetworkEntityService _entities;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _entities);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _entities = default;
        }
        #endregion

        #region IMessageProcessor<CreateNetworkEntityMessage> Implementation
        void IMessageProcessor<CreateNetworkEntityMessage>.Process(CreateNetworkEntityMessage message)
        {
            if(!_entities.TryProcess(message))
            {
                throw new InvalidOperationException();
            }
        }
        #endregion
    }
}
