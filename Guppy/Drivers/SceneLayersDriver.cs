using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Drivers
{
    [IsDriver(typeof(Scene))]
    /// <summary>
    /// Driver used to automatically update
    /// a scene's layers
    /// </summary>
    public class SceneLayersDriver : Driver
    {
        #region Private Fields
        private LayerCollection _layers;
        #endregion

        #region Constructors
        public SceneLayersDriver(LayerCollection layers)
        {
            _layers = layers;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _layers.TryUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _layers.TryDraw(gameTime);
        }
        #endregion
    }
}
