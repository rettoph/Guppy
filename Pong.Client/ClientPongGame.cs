using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Scenes;
using Pong.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client
{
    class ClientPongGame : PongGame
    {
        public ClientPongGame(IServiceProvider provider) : base(provider)
        {
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            var client = this.provider.GetService<ClientPeer>();
            client.Start();

            // Create a new lobby scene
            this.scenes.Create<ClientLoginScene>();
        }
    }
}
