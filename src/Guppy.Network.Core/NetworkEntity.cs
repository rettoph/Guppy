using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Interfaces;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public class NetworkEntity : Entity, INetworkEntity
    {
        #region Public Properties
        public UInt16 NetworkId { get; set; }
        public NetworkEntityMessageService Messages { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Messages = provider.GetService<NetworkEntityMessageService>();
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Messages.TryRelease();
            this.Messages = default;
        }
        #endregion
    }
}
