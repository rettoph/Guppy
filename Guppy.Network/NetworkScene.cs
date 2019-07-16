﻿using Guppy.Network.Collections;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        protected internal Queue<NetOutgoingMessage> priorityActionQueue;

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
            this.priorityActionQueue = new Queue<NetOutgoingMessage>();

            this.Group.MessageHandler["action"] = this.HandleActionMessage;
        }
        #endregion

        protected override void update(GameTime gameTime)
        {
            base.update(gameTime);

            // Push all action messages to the connected peer...
            if (this.Group.Users.Count() > 0)
            {
                // Auto handle any recieved actions
                while (this.priorityActionQueue.Count > 0)
                    this.Group.SendMesssage(this.priorityActionQueue.Dequeue(), NetDeliveryMethod.ReliableOrdered, 0);
                while (this.actionQueue.Count > 0)
                    this.Group.SendMesssage(this.actionQueue.Dequeue(), NetDeliveryMethod.UnreliableSequenced, 0);
            }
            else
            {
                this.priorityActionQueue.Clear();
                this.actionQueue.Clear();
            }
        }

        #region NetMessage Handlers
        protected internal void HandleActionMessage(NetIncomingMessage obj)
        {
            var id = obj.ReadGuid();
            var type = obj.ReadString();

            var ne = this.networkEntities.GetById(id);
            
            if(ne == null)
            {
                this.logger.LogError($"Unable to run action message. Unknown NetworkEntity({id}) => Type: '{type}'");
                return;
            }

            ne.HandleAction(type, obj);
        }
        #endregion
    }
}
