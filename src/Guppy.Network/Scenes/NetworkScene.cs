using Guppy.DependencyInjection;
using Guppy.Network.Interfaces;
using Guppy.Network.Peers;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network.Scenes
{
    public abstract class NetworkScene : Scene
    {
        #region Protected Properties
        protected Peer peer { get; private set; }
        #endregion

        #region Public Properties
        public IChannel Channel { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.peer = provider.GetService<Peer>();
            this.Channel = this.GetChannel(this.peer);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.peer = null;
            this.Channel = null;
        }
        #endregion

        #region Methods
        protected abstract IChannel GetChannel(Peer peer);
        #endregion
    }
}
