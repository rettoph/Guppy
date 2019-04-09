using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.UI.Elements;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities
{
    public class Stage : DebuggableEntity
    {
        private GameWindow _window;
        private Style _stageStyle;

        public StageContainer Content { get; private set; }

        protected internal UnitRectangle clientBounds { get; private set; }
        protected internal InputManager inputManager;
        protected internal RenderTarget2D internalRenderTarget { get; private set; }
        protected internal GraphicsDevice graphicsDevice { get; private set; }
        protected internal SpriteBatch internalSpriteBatch { get; private set; }

        private SpriteBatch _spriteBatch;

        #region Constructors
        public Stage(
            InputManager inputManager,
            SpriteBatch spriteBatch,
            GameWindow window,
            GraphicsDevice graphicsDevice,
            EntityConfiguration configuration,
            Scene scene,
            ILogger logger) : base(configuration, scene, logger)
        {
            _window = window;
            _spriteBatch = spriteBatch;
            this.inputManager = inputManager;

            this.clientBounds = new UnitRectangle(0, 0, _window.ClientBounds.Width - 1, _window.ClientBounds.Height - 1);
            this.Content = new StageContainer(this);

            this.graphicsDevice = graphicsDevice;
            this.internalRenderTarget = new RenderTarget2D(this.graphicsDevice, _window.ClientBounds.Width, _window.ClientBounds.Height);
            this.internalSpriteBatch = new SpriteBatch(this.graphicsDevice);

            var style = new Style();
            var test = new SimpleContainer(0.25f, 0.25f, 0.5f, 0.5f, style);
            this.Content.Add(test);

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;
        }
        #endregion

        #region Frame Methods
        public override void Draw(GameTime gameTime)
        {
            this.Content.Draw(_spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            this.Content.Update(gameTime);
        }
        public override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            this.clientBounds.Update();

            this.Content.AddDebugVertices(ref vertices);
        }
        #endregion

        #region Event Handlers
        private void HandleClientBoundsChanged(object sender, EventArgs e)
        {
            this.clientBounds.Width.SetValue(_window.ClientBounds.Width - 1);
            this.clientBounds.Height.SetValue(_window.ClientBounds.Height - 1);

            this.internalRenderTarget?.Dispose();
            this.internalRenderTarget = new RenderTarget2D(this.graphicsDevice, _window.ClientBounds.Width, _window.ClientBounds.Height);
        }
        #endregion
    }
}
