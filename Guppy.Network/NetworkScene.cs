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
    public abstract class NetworkScene<TGroup> : Scene
        where TGroup : Group
    {
        protected NetworkEntityCollection networkEntities { get; private set; }
        protected TGroup group { get; private set; }
        protected NetworkSceneDriver driver { get; set; }

        public NetworkScene(TGroup group, IServiceProvider provider) : base(provider)
        {
            this.group = group;
            this.driver = provider.GetRequiredService<NetworkSceneDriver>();
        }

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            this.networkEntities = new NetworkEntityCollection(this.entities);

            this.driver.Setup(this);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            this.driver.Update(this, this.group, this.networkEntities);

            base.Update(gameTime);
        }
    }
}
