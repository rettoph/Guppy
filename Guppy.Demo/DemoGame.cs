using Guppy.Attributes;
using Guppy.Demo.Scenes;
using Guppy.Extensions.DependencyInjection;
using Guppy.Network.Peers;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Guppy.Demo
{
    [IsGame]
    public class DemoGame : Guppy.Game
    {
        private DemoScene _scene;
        private ClientPeer _client;

        public DemoGame(ClientPeer client)
        {
            _client = client;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _client.Start();
            _scene = this.provider.GetScene<DemoScene>();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _scene.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _client.TryUpdate(gameTime);
            _scene.TryUpdate(gameTime);
        }
    }
}
