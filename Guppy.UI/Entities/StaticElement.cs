using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.UI.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities
{
    public class StaticElement : Element
    {
        public StaticElement(Rectangle bounds, InputManager inputManager, SpriteBatch spriteBatch, GameWindow window, GraphicsDevice graphiceDevice, EntityConfiguration configuration, Scene scene, ILogger logger, string text = "") : base(bounds, inputManager, spriteBatch, window, graphiceDevice, configuration, scene, logger, text)
        {
            this.TrackState = false;
        }
        public StaticElement(UnitRectangle bounds, InputManager inputManager, SpriteBatch spriteBatch, GameWindow window, GraphicsDevice graphiceDevice, EntityConfiguration configuration, Scene scene, ILogger logger, string text = "") : base(bounds, inputManager, spriteBatch, window, graphiceDevice, configuration, scene, logger, text)
        {
            this.TrackState = false;
        }
    }
}
