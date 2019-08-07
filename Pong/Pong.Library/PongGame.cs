using Guppy;
using Guppy.Attributes;
using Guppy.Network.Peers;
using Microsoft.Xna.Framework;
using System;
using Game = Guppy.Game;

namespace Pong.Library
{
    /// <summary>
    /// The main pong game class.
    /// </summary>
    public class PongGame : Game
    {
        private Peer _peer;

        public PongGame(Peer peer)
        {
            _peer = peer;
        }

        protected override void Update(GameTime gameTime)
        {
            // Automatically update the peer
            _peer.TryUpdate(gameTime);

            base.Update(gameTime);
        }
    }
}
