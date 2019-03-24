using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities
{
    /// <summary>
    /// The stage is a root container for a UI.
    /// All buttons, containers, inputs, and other
    /// elements must reside within a stage.
    /// </summary>
    public class Stage : Entity
    {
        #region Private Fields
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;
        private GameWindow _window;
        #endregion

        #region Public Attributes
        /// <summary>
        /// When true, a wireframe of the stage and all contained elements
        /// will be rendered on draw.
        /// </summary>
        public Boolean Debug { get; set; }
        #endregion

        public Stage(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, GameWindow window, EntityConfiguration configuration, Scene scene, ILogger logger) : base(configuration, scene, logger)
        {
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;
            _window = window;
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
