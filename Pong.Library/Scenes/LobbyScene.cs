using Guppy.Network;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Pong.Library.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Library.Scenes
{
    public abstract class LobbyScene : NetworkScene
    {
        protected Group group;

        public LobbyScene(Group group, IServiceProvider provider) : base(provider)
        {
            this.group = group;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.layers.Create<SimpleLayer>();

            this.group.MessageHandler.Add("chat", this.HandleChatMessage);
        }

        public override void Update(GameTime gameTime)
        {
            this.group.Update();

            base.Update(gameTime);
        }

        protected abstract void HandleChatMessage(NetIncomingMessage im);
    }
}
