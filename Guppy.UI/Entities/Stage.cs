using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.Loaders;
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
        private RenderTargetBinding[] _cachedRenderTargets;
        private Boolean _startedInternalGraphics;

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
            IServiceProvider provider,
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
            style.Set<Texture2D>(ElementState.Normal, StyleProperty.BackgroundImage, provider.GetLoader<ContentLoader>().Get<Texture2D>("button"));
            style.Set<DrawType>(ElementState.Normal, StyleProperty.BackgroundType, DrawType.Tile);

            for (Int32 i = 0; i < 10; i++)
            {
                var test = new SimpleContainer(100, i * 35, 200, 30, style);
                this.Content.Add(test);
            }


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

            // If this is called, then we must reset the render target
            if(_startedInternalGraphics)
            {
                this.graphicsDevice.SetRenderTargets(_cachedRenderTargets);
                _cachedRenderTargets = null;
                _startedInternalGraphics = false;
            }
        }
        public override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            this.clientBounds.Update();

            this.Content.AddDebugVertices(ref vertices);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        protected internal void startInternalGraphics()
        {
            if (!_startedInternalGraphics) {
                _cachedRenderTargets = this.graphicsDevice.GetRenderTargets();

                this.graphicsDevice.SetRenderTarget(this.internalRenderTarget);

                _startedInternalGraphics = true;
            }
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
