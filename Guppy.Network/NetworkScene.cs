using Guppy.Network.Collections;
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

        public NetworkScene(Group group, IServiceProvider provider) : base(provider)
        {
            this.Group = group;
            this.networkEntities = provider.GetRequiredService<NetworkEntityCollection>();
        }

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            this.Group.Messages.AddHandler("action", this.HandleActionMessage);
        }
        #endregion

        protected override void update(GameTime gameTime)
        {
            base.update(gameTime);
        }

        #region NetMessage Handlers
        protected internal void HandleActionMessage(NetIncomingMessage obj)
        {
            var id = obj.ReadGuid();

            var ne = this.networkEntities.GetById(id);
            
            if(ne == null)
            {
                this.logger.LogError($"Unable to run action message. Unknown NetworkEntity({id}) => Type: '{obj.ReadString()}'");
                return;
            }

            ne.Actions.HandleMessage(obj);
        }
        #endregion
    }
}
