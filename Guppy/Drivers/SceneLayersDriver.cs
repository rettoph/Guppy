using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Enums;
using Guppy.Implementations;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Drivers
{
    /// <summary>
    /// Simple driver used to update a scene's layers.
    /// 
    /// This is within a driver so that custom drivers can be added to
    /// pre and post layer frame calls
    /// </summary>
    [IsDriver(typeof(Scene))]
    public class SceneLayersDriver : Driver<Scene>
    {
        #region Private Fields
        private LayerCollection _layers;
        #endregion

        #region Constructor
        public SceneLayersDriver(LayerCollection layers)
        {
            _layers = layers;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.UpdateOrder = 100;
            this.DrawOrder = 100;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _layers.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _layers.TryUpdate(gameTime);
        }
        #endregion
    }
}
