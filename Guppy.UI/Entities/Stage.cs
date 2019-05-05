using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.UI.Elements;
using Guppy.UI.Enums;
using Guppy.UI.Loaders;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Extensions.DependencyInjection;
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
        
        private RenderTarget2D _layerRenderTarget;
        private RenderTarget2D _outputRenderTarget;
        private SpriteBatch _internalSpriteBatch;
        private SpriteBatch _spriteBatch;
        private LayerCollection _layers;

        private IServiceProvider _provider;
        #endregion

        #region Protected Internal Fields
        protected internal StyleLoader styleLoader;

        protected internal GraphicsDevice graphicsDevice;

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

        #region Events
        public event EventHandler<TextInputEventArgs> TextInput {
            add { _window.TextInput += value; }
            remove { _window.TextInput -= value; }
        }
        #endregion

        #region Constructors
        public Stage(
            SpriteBatch spriteBatch,
            GameWindow window,
            GraphicsDevice graphicsDevice,
            EntityConfiguration configuration,
            Scene scene,
            LayerCollection layers,
            IServiceProvider provider,
            ILogger logger) : base(configuration, scene, logger)
        {
            _layers = layers;
            _window = window;
            _provider = provider;
            this.graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _internalSpriteBatch = new SpriteBatch(this.graphicsDevice);
            _layerRenderTarget = new RenderTarget2D(this.graphicsDevice, _window.ClientBounds.Width, _window.ClientBounds.Height);
            _outputRenderTarget = new RenderTarget2D(this.graphicsDevice, _window.ClientBounds.Width, _window.ClientBounds.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

            this.dirtyTextureElementQueue = new Queue<Element>();
            this.clientBounds = new UnitRectangle(0, 0, _window.ClientBounds.Width, _window.ClientBounds.Height);
            this.font = _provider.GetLoader<ContentLoader>().Get<SpriteFont>("ui:font");
            this.styleLoader = _provider.GetLoader<StyleLoader>();

            var style = new Style();
            style.Set<Color>(ElementState.Normal, StateProperty.OuterDebugColor, Color.Red);
            style.Set<Color>(ElementState.Hovered, StateProperty.OuterDebugColor, Color.Blue);
            style.Set<Color>(ElementState.Pressed, StateProperty.OuterDebugColor, Color.Green);
            style.Set<Color>(ElementState.Active, StateProperty.OuterDebugColor, Color.Orange);

            this.Content = new StageContent(new UnitRectangle(0, 0, 1f, 1f), this);
            this.Content.Outer.setParent(this.clientBounds);

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
            var camera = _layers.GetLayer(this.LayerDepth).Camera;
            var mState = Mouse.GetState();
            var mPos = Vector2.Transform(mState.Position.ToVector2(), Matrix.Invert(camera.World));
            this.Content.Update(new MouseState(
                x: (Int32)mPos.X,
                y: (Int32)mPos.Y,
                scrollWheel: mState.ScrollWheelValue,
                leftButton: mState.LeftButton,
                middleButton: mState.MiddleButton,
                rightButton: mState.RightButton,
                xButton1: mState.XButton1,
                xButton2: mState.XButton2,
                horizontalScrollWheel: mState.HorizontalScrollWheelValue));

            if(this.dirtyTextureElementQueue.Count > 0)
            { // If there are any dirty elements...
                // Cache the current render targets...
                var renderTargetsCache = this.graphicsDevice.GetRenderTargets();

                // Clean any self contained dirty textures
                while (this.dirtyTextureElementQueue.Count > 0)
                    this.dirtyTextureElementQueue.Dequeue().CleanTexture(
                        this.graphicsDevice, 
                        _layerRenderTarget, 
                        _outputRenderTarget, 
                        _internalSpriteBatch);



                // Reset the graphics device render targets
                this.graphicsDevice.SetRenderTargets(renderTargetsCache);
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
            this.clientBounds.Width.SetValue(_window.ClientBounds.Width);
            this.clientBounds.Height.SetValue(_window.ClientBounds.Height);

            this.clientBounds.Update();

            _layerRenderTarget?.Dispose();
            _outputRenderTarget?.Dispose();

            _layerRenderTarget = new RenderTarget2D(this.graphicsDevice, _window.ClientBounds.Width, _window.ClientBounds.Height);
            _outputRenderTarget = new RenderTarget2D(this.graphicsDevice, _window.ClientBounds.Width, _window.ClientBounds.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }
        #endregion

        #region Create Methods
        protected internal TElement CreateElement<TElement>(UnitValue x, UnitValue y, UnitValue width, UnitValue height, Element parent, params Object[] args)
            where TElement : Element
        {
            List<Object> eArgs = new List<Object>();
            eArgs.Add(new UnitRectangle(x, y, width, height));
            eArgs.Add(this);
            eArgs.Add(parent);
            eArgs.AddRange(args);

            return ActivatorUtilities.CreateInstance<TElement>(_provider, eArgs.ToArray());
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= this.HandleClientBoundsChanged;

            _internalSpriteBatch.Dispose();
            _layerRenderTarget.Dispose();
            _outputRenderTarget.Dispose();

            this.clientBounds.Dispose();

            this.dirtyTextureElementQueue.Clear();

            this.Content.Dispose();
        }
    }
}
