using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    public abstract class Element : UnitRectangle
    {
        #region Static Fields
        private static ElementState[] States;

        #endregion

        #region Private Fields
        private Dictionary<ElementState, Texture2D> _textures;
        private Dictionary<ElementState, VertexPositionColor[]> _debugVertices;
        private Boolean _hasLeftButtonBeenUp;
        #endregion

        #region Protected Attributes
        protected Boolean mouseOver { get; private set; }
        protected SpriteBatch internalSpriteBatch { get { return this.Stage.internalSpriteBatch; } }
        protected GraphicsDevice graphicsDevice { get { return this.Stage.graphicsDevice; } }
        protected InputManager inputManager { get { return this.Stage.inputManager; } }
        protected GameWindow window { get { return this.Stage.window; } }
        #endregion

        #region Public Attributes
        public abstract Stage Stage { get; }
        public Element Parent { get; protected internal set; }
        public ElementState State { get; protected set; }
        public ElementStyleSheet StyleSheet { get; private set; }
        public Boolean Dirty { get; set; }
        #endregion

        #region Events
        public event EventHandler<Element> OnMouseEnter;
        public event EventHandler<Element> OnMouseExit;
        public event EventHandler<Element> OnMouseDown;
        public event EventHandler<Element> OnMouseUp;
        public event EventHandler<Element> OnActivated;
        public event EventHandler<Element> OnDeactivated;
        #endregion

        #region Constructors
        public Element(Unit x, Unit y, Unit width, Unit height, StyleSheet rootStyleSheet = null) : base(x, y, width, height)
        {
            this.State = ElementState.Normal;
            this.StyleSheet = new ElementStyleSheet(rootStyleSheet, this);
            this.Dirty = false;

            _textures = new Dictionary<ElementState, Texture2D>(Element.States.Length);
            _debugVertices = new Dictionary<ElementState, VertexPositionColor[]>();
        }
        static Element()
        {
            Element.States = (ElementState[])Enum.GetValues(typeof(ElementState));
        }
        #endregion

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_textures[this.State], this.Bounds, _textures[this.State].Bounds, Color.White);;
        }

        public virtual void Update(GameTime gameTime)
        {
            this.mouseOver = this.Bounds.Contains(inputManager.Mouse.Position);

            if (this.mouseOver && this.State == ElementState.Normal)
            { // If mouse enter...
                this.State = ElementState.Hovered;
                this.OnMouseEnter?.Invoke(this, this);

                _hasLeftButtonBeenUp = inputManager.Mouse.LeftButton == ButtonState.Released;
            }
            else if (!this.mouseOver && (this.State != ElementState.Normal && this.State != ElementState.Active))
            { // If mouse exit...
                if(this.State != ElementState.Active)
                    this.State = ElementState.Normal;

                this.OnMouseExit?.Invoke(this, this);
            }
            else if (this.mouseOver && (this.State == ElementState.Hovered || this.State == ElementState.Active) && inputManager.Mouse.LeftButton == ButtonState.Pressed && _hasLeftButtonBeenUp)
            { // If mouse down...
                this.State = ElementState.Pressed;
                this.OnMouseDown?.Invoke(this, this);
            }
            else if (this.mouseOver && this.State == ElementState.Pressed && inputManager.Mouse.LeftButton == ButtonState.Released)
            { // If mouse up...
                this.State = ElementState.Active;
                this.OnActivated?.Invoke(this, this);
                this.OnMouseUp?.Invoke(this, this);
            }
            else if (this.mouseOver && !_hasLeftButtonBeenUp && inputManager.Mouse.LeftButton == ButtonState.Released)
            { // If mouse up (when it was down on hover)
                _hasLeftButtonBeenUp = inputManager.Mouse.LeftButton == ButtonState.Released;
            }
            else if(!this.mouseOver && this.State == ElementState.Active && inputManager.Mouse.LeftButton == ButtonState.Pressed)
            { // If mouse inactive
                this.State = ElementState.Normal;
                this.OnDeactivated?.Invoke(this, this);
            }

            if (this.Dirty)
                this.UpdateCache();
        }

        protected internal virtual void UpdateCache()
        {
            if (this.Parent != null)
                this.UpdateBounds(this.Parent);

            // Store a list of the graphic device's original render targets...
            var initialRenderTargets = this.graphicsDevice.GetRenderTargets();

            foreach (ElementState state in Element.States)
            {
                // First dispose of the old texture for this state...
                if (_textures.ContainsKey(state))
                    _textures[state]?.Dispose();

                // Create a new render target and set the graphics device target to it...
                if (this.Bounds.Width > 0 && this.Bounds.Height > 0)
                {
                    var renderTarget = new RenderTarget2D(this.graphicsDevice, this.Bounds.Width, this.Bounds.Height);
                    this.graphicsDevice.SetRenderTarget(renderTarget);
                    this.graphicsDevice.Clear(Color.Transparent);
                    // Generate a new texture for this state...
                    this.generateTexture(state, ref renderTarget);
                    _textures[state] = renderTarget;
                }

                // Generate new debug vertices for the element..
                _debugVertices[state] = this.generateDebugVertices(state);
            }

            // After all the new state targets have generated...
            // reset the render targets
            this.graphicsDevice.SetRenderTargets(initialRenderTargets);

            // Clean the current element
            this.Dirty = false;
        }

        protected internal virtual void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            vertices.AddRange(_debugVertices[this.State]);
        }

        /// <summary>
        /// Generate the texture for a given element state...
        /// </summary>
        /// <param name="state"></param>
        protected virtual void generateTexture(ElementState state, ref RenderTarget2D target)
        {
            this.graphicsDevice.Clear(Color.Transparent);

            var background = this.StyleSheet.GetProperty<Texture2D>(state, StyleProperty.BackgroundImage);

            if (background != null)
            {
                this.internalSpriteBatch.Begin();
                this.internalSpriteBatch.Draw(background, target.Bounds, Color.White);
                this.internalSpriteBatch.End();
            }
        }

        /// <summary>
        /// Generate a list of vertices for the stage debug view
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual VertexPositionColor[] generateDebugVertices(ElementState state)
        {
            var wireframeColor = this.StyleSheet.GetProperty<Color>(state, StyleProperty.DebugWireframeColor);

            return new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), wireframeColor)
            };
        }
    }
}
