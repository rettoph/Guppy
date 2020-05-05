using Guppy.DependencyInjection;
using Guppy.UI.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Backgrounds
{
    /// <summary>
    /// Abstract class outlining what UI element's UI.
    /// </summary>
    public abstract class Background
    {
        public virtual void Setup(ServiceProvider graphics, IElement element)
        {
            //
        }

        /// <summary>
        /// Render the current background.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="primitiveBatch"></param>
        /// <param name="element"></param>
        /// <param name="gameTime"></param>
        public abstract void Draw(GameTime gameTime);

        public static implicit operator Background(Color color)
        {
            return new ColorBackground()
            {
                Color = color
            };
        }
    }
}
