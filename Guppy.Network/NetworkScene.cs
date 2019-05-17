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
        protected NetworkEntityCollection networkEntities { get; private set; }
        protected internal Group group { get; private set; }
        protected NetworkSceneDriver driver { get; set; }

        /// <summary>
        /// Queue containing actions preformed since the last flush
        /// </summary>
        protected internal Queue<NetOutgoingMessage> actionQueue;

        public NetworkScene(Group group, IServiceProvider provider) : base(provider)
        {
            this.group = group;
            this.driver = provider.GetRequiredService<NetworkSceneDriver>();
        }

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            this.actionQueue = new Queue<NetOutgoingMessage>();
            this.networkEntities = new NetworkEntityCollection(this.entities);

            this.driver.Setup(this, this.group, networkEntities);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            this.driver.Update(this, this.group, this.networkEntities);

            base.Update(gameTime);
        }

        /// <summary>
        /// Public function automatically called whenever a user joins the server. Special note,
        /// this function is called withene a network scene driver.
        /// </summary>
        /// <param name="user"></param>
        public virtual void UserAdded(User user)
        {

        }
    }
}
