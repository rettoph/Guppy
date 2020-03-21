using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;

namespace Guppy
{
    /// <summary>
    /// The main Guppy game class. Manages scene instances.
    /// </summary>
    public abstract class Game : Frameable, IGame
    {
        #region Private Fields
        private UpdateBuffer _updateBuffer;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _updateBuffer = provider.GetService<UpdateBuffer>();
        }
        #endregion

        #region Frame Methods
        protected override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);

            _updateBuffer.Flush(gameTime);
        }
        #endregion
    }
}
