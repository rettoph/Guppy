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
    public abstract class NetworkLayerable : Layerable, INetworkEntity
    {
        #region Private Fields
        private Pipe _pipe;
        #endregion

        #region Public Properties
        public UInt16 NetworkId { get; set; }
        public NetworkEntityPacketService Messages { get; private set; }
        public virtual Pipe Pipe
        {
            get => _pipe;
            protected set => this.OnPipeChanged.InvokeIf(_pipe != value, this, ref _pipe, value);
        }

        Pipe INetworkEntity.Pipe
        {
            get => this.Pipe;
            set => this.Pipe = value;
        }
        #endregion

        #region Events
        public virtual event OnChangedEventDelegate<INetworkEntity, Pipe> OnPipeChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Messages = provider.GetService<NetworkEntityPacketService>();
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
