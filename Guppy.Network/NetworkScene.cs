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
        public Group Group { get; private set; }

        /// <summary>
        /// Queue containing actions preformed since the last flush
        /// </summary>
        protected internal Queue<NetOutgoingMessage> actionQueue;

        public NetworkScene(Group group, IServiceProvider provider) : base(provider)
        {
            this.Group = group;
            this.networkEntities = provider.GetRequiredService<NetworkEntityCollection>();
        }

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            this.actionQueue = new Queue<NetOutgoingMessage>();
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            this.Group.Update();

            base.Update(gameTime);

            // Push all action messages to the connected peer...
            if (this.Group.Users.Count() > 0)
            {
                // All drivers should auto-push any recieved actions
                while (this.actionQueue.Count > 0)
                    this.Group.SendMesssage(this.actionQueue.Dequeue(), NetDeliveryMethod.UnreliableSequenced);
            }
            else
            {
                this.actionQueue.Clear();
            }
        }
    }
}
