using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.UI.Configurations;
using Guppy.UI.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Guppy.UI.Utilities;

namespace Guppy.UI.Entities
{
    public class Element : Entity
    {
        private InputManager inputManager;
        private Boolean _hasLeftButtonBeenUp;
        private Dictionary<ElementState, RenderTarget2D> _textures;
        private Dictionary<ElementState, VertexPositionColor[]> _debugVertices;

        protected Boolean dirty;
        protected ElementConfiguration configuration;
        protected SpriteBatch spriteBatch;
        protected SpriteBatch internalSpriteBatch;
        protected GameWindow window;
        protected GraphicsDevice graphicsDevice;

        public UnitRectangle Bounds { get; private set; }
        public Boolean MouseOver { get; private set; }
        public ElementState State { get; private set; }

        public String Text { get; set; }
        public Boolean TrackState { get; set; }

        public event EventHandler<Element> OnMouseEnter;
        public event EventHandler<Element> OnMouseExit;
        public event EventHandler<Element> OnMouseDown;
        public event EventHandler<Element> OnMouseUp;

        #region Constructors
        public Element(
            Rectangle bounds, 
            InputManager inputManager,
            SpriteBatch spriteBatch,
            GameWindow window,
            GraphicsDevice graphiceDevice,
            EntityConfiguration configuration,
            Scene scene,
            ILogger logger,
            String text = "") : base(configuration, scene, logger)
        {
            this.Instantiate(bounds, inputManager, spriteBatch, window, graphiceDevice, configuration, scene, logger, text);
        }
        public Element(
            UnitRectangle bounds,
            InputManager inputManager,
            SpriteBatch spriteBatch,
            GameWindow window,
            GraphicsDevice graphicsDevice,
            EntityConfiguration configuration,
            Scene scene,
            ILogger logger,
            String text = "") : base(configuration, scene, logger)
        {
            this.Instantiate(bounds, inputManager, spriteBatch, window, graphicsDevice, configuration, scene, logger, text);
        }
        protected virtual void Instantiate(
            UnitRectangle bounds,
            InputManager inputManager,
            SpriteBatch spriteBatch,
            GameWindow window,
            GraphicsDevice graphicsDevice,
            EntityConfiguration configuration,
            Scene scene,
            ILogger logger,
            String text = "")
        {
            this.inputManager = inputManager;
            this.spriteBatch = spriteBatch;
            this.window = window;
            this.graphicsDevice = graphicsDevice;
            this.configuration = configuration.Data as ElementConfiguration;
            this.internalSpriteBatch = new SpriteBatch(graphicsDevice);
            _textures = new Dictionary<ElementState, RenderTarget2D>();
            _debugVertices = new Dictionary<ElementState, VertexPositionColor[]>();

            this.dirty = true;

            this.Bounds = bounds;
            this.Text = text;
            this.TrackState = true;

            this.State = ElementState.Normal;

            this.SetUpdateOrder(100);

            this.window.ClientSizeChanged += this.HandleClientSizeChanged;
        }
        #endregion



        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Draw(_textures[this.State], this.Bounds.Output, _textures[this.State].Bounds, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (this.TrackState)
            {
                this.MouseOver = this.Bounds.Contains(inputManager.Mouse.Position);

                if (this.MouseOver && this.State == ElementState.Normal)
                { // If mouse enter...
                    this.State = ElementState.Hovered;
                    this.OnMouseEnter?.Invoke(this, this);

                    _hasLeftButtonBeenUp = inputManager.Mouse.LeftButton == ButtonState.Released;
                }
                else if (!this.MouseOver && this.State != ElementState.Normal)
                { // If mouse exit...
                    this.State = ElementState.Normal;
                    this.OnMouseExit?.Invoke(this, this);
                }
                else if (this.MouseOver && this.State == ElementState.Hovered && inputManager.Mouse.LeftButton == ButtonState.Pressed && _hasLeftButtonBeenUp)
                { // If mouse down...
                    this.State = ElementState.Active;
                    this.OnMouseDown?.Invoke(this, this);
                }
                else if (this.MouseOver && this.State == ElementState.Active && inputManager.Mouse.LeftButton == ButtonState.Released)
                { // If mouse up...
                    this.State = ElementState.Hovered;
                    this.OnMouseUp?.Invoke(this, this);
                }
                else if (this.MouseOver && !_hasLeftButtonBeenUp && inputManager.Mouse.LeftButton == ButtonState.Released)
                { // If mouse up (when it was down on hover)
                    _hasLeftButtonBeenUp = inputManager.Mouse.LeftButton == ButtonState.Released;
                }
            }

            if (this.dirty)
                this.GenerateTextures();
        }

        /// <summary>
        /// Used to generate all textures for the current element.
        /// This is called on update if this.dirty is currently set
        /// to true. Dirty is automatically updated on text change,
        /// on bounds change, ect...
        /// </summary>
        protected virtual void GenerateTextures()
        {
            // Ensure the bounds rectangle gets updated now...
            this.Bounds.Update(window.ClientBounds);

            var originalRenderTargets = this.graphicsDevice.GetRenderTargets();
            var targetBounds = new Rectangle(0, 0, this.Bounds.Width, this.Bounds.Height);
            BaseElementConfiguration stateConfiguration;


            foreach (ElementState state in (ElementState[])Enum.GetValues(typeof(ElementState)))
            {
                // Dispose of the old texture
                if (_textures.ContainsKey(state))
                    _textures[state].Dispose();

                // Load the states configuration
                stateConfiguration = this.configuration.GetConfiguration(state);

                // Create a new render target for the current state
                _textures[state] = new RenderTarget2D(this.graphicsDevice, this.Bounds.Width, this.Bounds.Height);
                this.graphicsDevice.SetRenderTarget(_textures[state]);

                // Begin drawing the overlay
                this.internalSpriteBatch.Begin(SpriteSortMode.Deferred, null, this.configuration.SamplerState, null, null);

                #region Background Drawing...
                if(stateConfiguration.BackgroundTexture != null) // Draw the background onto the spritebatch
                    switch (stateConfiguration.BackgroundMethod)
                    {
                        case BackgroundMethod.Tile:
                            this.internalSpriteBatch.Draw(stateConfiguration.BackgroundTexture, targetBounds, targetBounds, Color.White);
                            break;
                        case BackgroundMethod.Stretch:
                            this.internalSpriteBatch.Draw(stateConfiguration.BackgroundTexture, targetBounds, stateConfiguration.BackgroundTexture.Bounds, Color.White);
                            break;
                    }
                #endregion

                this.internalSpriteBatch.End();

                #region Debug Vertice Creation
                _debugVertices[state] = new VertexPositionColor[] {
                    new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), stateConfiguration.DebugColor),
                    new VertexPositionColor(new Vector3(this.Bounds.Right - 1, this.Bounds.Top, 0), stateConfiguration.DebugColor),
                    new VertexPositionColor(new Vector3(this.Bounds.Right - 1, this.Bounds.Top, 0), stateConfiguration.DebugColor),
                    new VertexPositionColor(new Vector3(this.Bounds.Right - 1, this.Bounds.Bottom - 1, 0), stateConfiguration.DebugColor),
                    new VertexPositionColor(new Vector3(this.Bounds.Right - 1, this.Bounds.Bottom - 1, 0), stateConfiguration.DebugColor),
                    new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom - 1, 0), stateConfiguration.DebugColor),
                    new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom - 1, 0), stateConfiguration.DebugColor),
                    new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), stateConfiguration.DebugColor),
                };
                #endregion
            }

            // reset the graphics device render targets
            this.graphicsDevice.SetRenderTargets(originalRenderTargets);

            this.dirty = false;
        }

        protected virtual internal VertexPositionColor[] GetDebugVertices()
        {
            return _debugVertices[this.State];
        }

        public override void Dispose()
        {
            base.Dispose();

            this.window.ClientSizeChanged -= this.HandleClientSizeChanged;

            foreach (KeyValuePair<ElementState, RenderTarget2D> kvp in _textures)
                kvp.Value.Dispose();
        }

        #region Event Handlers
        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            this.dirty = true;
        }
        #endregion
    }
}
