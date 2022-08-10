using Guppy.Example.Library;
using Guppy.MonoGame.Services;
using Guppy.Network.Peers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Server
{
    public class ServerExampleGuppy : ExampleGuppy
    {
        private ServerPeer _server;

        public ServerExampleGuppy(ServerPeer server, ITerminalService terminal, World world) : base(terminal, world)
        {
            _server = server;

            _server.Start(1337);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _server.Flush();
        }
    }
}
