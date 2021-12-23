using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Interfaces;
using Guppy.Network.Services;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetworkEntity : Entity, INetworkEntity
    {
        #region Private Fields
        private Pipe _pipe;
        #endregion

        #region Public Properties
        public UInt16 NetworkId { get; set; }
        public NetworkEntityPacketService Packets { get; private set; }
        public Pipe Pipe
        {
            get => _pipe;
            set => this.OnPipeChanged.InvokeIf(_pipe != value, this, ref _pipe, value);
        }
        #endregion

        #region Events
        public event OnChangedEventDelegate<INetworkEntity, Pipe> OnPipeChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Packets = provider.GetService<NetworkEntityPacketService>();
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Packets.TryRelease();
            this.Packets = default;
        }
        #endregion
    }
}
