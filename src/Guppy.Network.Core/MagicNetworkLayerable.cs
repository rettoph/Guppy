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
    public abstract class MagicNetworkLayerable : NetworkLayerable, IMagicNetworkEntity
    {
        #region Private Fields
        private Pipe _pipe;
        #endregion

        #region Public Properties
        public NetworkEntityMessageService Messages { get; private set; }

        public virtual Pipe Pipe
        {
            get => _pipe;
            protected set => this.OnPipeChanged.InvokeIf(_pipe != value, this, ref _pipe, value);
        }

        Pipe IMagicNetworkEntity.Pipe
        {
            get => this.Pipe;
            set => this.Pipe = value;
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Messages = provider.GetService<NetworkEntityMessageService>((m, _, _) => m.entity = this);
        }

        protected override void PostUninitialize()
        {
            base.PostUninitialize();

            this.Messages.Dispose();
        }
        #endregion

        #region Events
        public virtual event OnChangedEventDelegate<IMagicNetworkEntity, Pipe> OnPipeChanged;
        #endregion
    }
}
