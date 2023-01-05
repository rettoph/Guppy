﻿using Guppy.Example.Library;
using Guppy.MonoGame.Services;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Network.Peers;
using Guppy.Network.Identity.Claims;
using Guppy.MonoGame.UI.Services;
using Guppy.Network;
using Guppy.Resources.Providers;
using Guppy.Example.Library.Constants;

namespace Guppy.Example.Client
{
    public sealed class ClientExampleGuppy : ExampleGuppy
    {
        private IInputService _inputs;
        private ClientPeer _client;

        public ClientExampleGuppy(IResourceProvider resources, ClientPeer client, NetScope scope, IInputService inputs, ITerminalService terminal, IDebuggerService debugger, World world) : base(scope, terminal, debugger, world)
        {

            var test = resources.Get<Color>(ColorConstants.ShipColor1);

            _inputs = inputs;
            _client = client;

            _client.Start();
            _client.Connect("localhost", 1337, Claim.Public("name", "Rettoph"), Claim.Public("age", 24));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _inputs.Update(gameTime);

            _client.Flush();
        }
    }
}