using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Implementations;
using Guppy.Network.Peers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Drivers
{
    [IsDriver(typeof(Peer))]
    public class PeerEntityDriver : Driver<Peer>
    {
        #region Private Fields
        private EntityCollection _entities;
        #endregion

        #region Constructor
        public PeerEntityDriver(EntityCollection entities)
        {
            _entities = entities;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _entities.TryUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _entities.TryDraw(gameTime);
        }
        #endregion
    }
}
