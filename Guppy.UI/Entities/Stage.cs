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
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI.Entities
{
    public class Stage : DebuggableEntity
    {
        #region Private Fields
        private GameWindow _window;
        private GraphicsDevice _graphicsDevice;
        private RenderTarget2D _layerRenderTarget;
        private RenderTarget2D _outputRenderTarget;
        private SpriteBatch _internalSpriteBatch;
        private SpriteBatch _spriteBatch;
        #endregion

        #region Protected Internal Fields
        /// <summary>
        /// Any elements containing dirty textures will
        /// be added to this queue. The stage will then
        /// manage the cleaning of all dirty textures post
        /// update.
        /// </summary>
        protected internal Queue<Element> dirtyTextureElementQueue;

        /// <summary>
        /// The bounds of the current client window
        /// </summary>
        protected internal UnitRectangle clientBounds { get; private set; }

        protected internal SpriteFont font;
        #endregion

        #region Public Attributes
        public Container Content { get; private set; }
        #endregion

        #region Constructors
        public Stage(
            SpriteBatch spriteBatch,
            GameWindow window,
            GraphicsDevice graphicsDevice,
            EntityConfiguration configuration,
            Scene scene,
            IServiceProvider provider,
            ILogger logger) : base(configuration, scene, logger)
        {
            _window = window;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _internalSpriteBatch = new SpriteBatch(_graphicsDevice);

            this.dirtyTextureElementQueue = new Queue<Element>();
            this.clientBounds = new UnitRectangle(0, 0, _window.ClientBounds.Width - 1, _window.ClientBounds.Height - 1);
            this.font = provider.GetLoader<ContentLoader>().Get<SpriteFont>("ui:font");

            var style = new Style();
            style.Set<UnitValue>(GlobalProperty.PaddingTop, 15);
            style.Set<UnitValue>(GlobalProperty.PaddingRight, 15);
            style.Set<UnitValue>(GlobalProperty.PaddingBottom, 15);
            style.Set<UnitValue>(GlobalProperty.PaddingLeft, 15);
            style.Set<Color>(ElementState.Normal, StateProperty.OuterDebugColor, Color.Red);
            style.Set<Color>(ElementState.Hovered, StateProperty.OuterDebugColor, Color.Blue);
            style.Set<Color>(ElementState.Pressed, StateProperty.OuterDebugColor, Color.Green);
            style.Set<Color>(ElementState.Active, StateProperty.OuterDebugColor, Color.Orange);

            this.Content = new StageContent(this, 0, 0, 1f, 1f, style);
            this.Content.Outer.setParent(this.clientBounds);

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;

            var bStyle = new Style();
            bStyle.Set<Texture2D>(ElementState.Normal, StateProperty.Background, provider.GetLoader<ContentLoader>().Get<Texture2D>("demo-button"));
            bStyle.Set<Texture2D>(ElementState.Hovered, StateProperty.Background, provider.GetLoader<ContentLoader>().Get<Texture2D>("demo-button-pressed"));
            bStyle.Set<Alignment>(ElementState.Normal, StateProperty.TextAlignment, Alignment.Center);
            bStyle.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.White);

            var it = new ScrollContainer(10, 10, 100, 300);
            var ft = new FancyTextElement(0, 0, 1f, 0.75f, bStyle);
            it.Items.Add(ft);
            it.Items.Add(new TextElement("test", 0, 0, 1f, 0.75f, bStyle));

            ft.Add("H", Color.Red);
            ft.Add("e", Color.Orange);
            ft.Add("l", Color.Yellow);
            ft.Add("l", Color.Green);
            ft.Add("o", Color.Blue);
            ft.Add("!", Color.Indigo);
            ft.Add("!", Color.Violet);

            this.Content.Add(it);
        }
        #endregion

        #region Frame Methods
        public override void Draw(GameTime gameTime)
        {
            this.Content.Draw(_spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            this.Content.Update(Mouse.GetState());

            if(this.dirtyTextureElementQueue.Count > 0)
            { // If there are any dirty elements...
                // Cache the current render targets...
                var renderTargetsCache = _graphicsDevice.GetRenderTargets();

                _layerRenderTarget = new RenderTarget2D(_graphicsDevice, _window.ClientBounds.Width, _window.ClientBounds.Height);
                _outputRenderTarget = new RenderTarget2D(_graphicsDevice, _window.ClientBounds.Width, _window.ClientBounds.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

                // Clean any self contained dirty textures
                while (this.dirtyTextureElementQueue.Count > 0)
                    this.dirtyTextureElementQueue.Dequeue().CleanTexture(
                        _graphicsDevice, 
                        _layerRenderTarget, 
                        _outputRenderTarget, 
                        _internalSpriteBatch);

                _layerRenderTarget?.Dispose();
                _outputRenderTarget?.Dispose();

                // Reset the graphics device render targets
                _graphicsDevice.SetRenderTargets(renderTargetsCache);
            }

        }
        public override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            this.Content.AddDebugVertices(ref vertices);
        }
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        private void HandleClientBoundsChanged(object sender, EventArgs e)
        {
            this.clientBounds.Width.SetValue(_window.ClientBounds.Width - 1);
            this.clientBounds.Height.SetValue(_window.ClientBounds.Height - 1);

            this.clientBounds.Update();
        }
        #endregion
    }
}
