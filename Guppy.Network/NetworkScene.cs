using Guppy.Network.Collections;
using Guppy.Network.Groups;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public abstract class NetworkScene : Scene
    {
        private Queue<NetworkEntity> _createdEntities;
        protected NetworkEntityCollection networkEntities;

        public NetworkScene(IServiceProvider provider) : base(provider)
        {
        }

        #region Initialization Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            _createdEntities = new Queue<NetworkEntity>();
            this.networkEntities = new NetworkEntityCollection(this.entities);
        }
        #endregion
    }
}
