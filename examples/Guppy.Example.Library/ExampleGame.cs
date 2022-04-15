using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Guppy.Gaming;
using Guppy.Gaming.Services;
using Guppy.Network.Peers;
using Guppy.Threading;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library
{
    public class ExampleGame : Game
    {
        private Bus _bus;
        private Peer _peer;

        public ExampleGame(
            Bus bus, 
            Peer peer,
            ISceneService scenes, 
            IEntityService entities) : base(scenes, entities)
        {
            _bus = bus;
            _peer = peer;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _peer.PollEvents();
            _bus.PublishEnqueued();

            base.Update(gameTime);
        }
    }
}
