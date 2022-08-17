using Guppy.ECS.Services;
using Guppy.Example.Library;
using Guppy.Example.Library.Constants;
using Guppy.MonoGame.Services;
using Guppy.Network;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Identity.Services;
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
        private IEntityService _entities;

        public ServerExampleGuppy(IEntityService entities, ServerPeer server, NetScope scope, ITerminalService terminal, IDebuggerService debugger, World world) : base(scope, terminal, debugger, world)
        {
            _server = server;
            _entities = entities;

            _server.Start(1337);

            _server.Users.OnUserConnected += this.HandleUserConnected;
            this.scope.Users.OnUserJoined += this.HandleUserJoined;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _server.Flush();
        }

        private void HandleUserConnected(IUserProvider sender, User args)
        {
            this.scope.Users.Add(args);
        }

        private void HandleUserJoined(IUserService sender, User user)
        {
            var paddle = _entities.Create(EntityConstants.Ship, user);
        }
    }
}
