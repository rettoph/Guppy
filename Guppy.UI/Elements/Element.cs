﻿using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Elements are the basic-most UI element.
    /// They contain nothing but inner bounds,
    /// outer bounds, state, and interfacing for
    /// more advanced custom UI elements.
    /// </summary>
    public class Element
    {
        #region Private Fields
        private List<Element> _children;

        private List<VertexPositionColor> _vertices;

        private Texture2D _texture;
        #endregion

        #region Protected Fields
        /// <summary>
        /// Contains layers to be drawn within the current element.
        /// Custom layers can be added at anytime, and next time the
        /// texture is cleaned, the layer will be ran then appended
        /// to the input texture.
        /// </summary>
        protected List<Func<SpriteBatch, Rectangle>> layers;
        #endregion

        #region Public Attributes
        public Stage Stage { get; protected internal set; }

        /// <summary>
        /// The current element's immediate parent, if any.
        /// </summary>
        public Element Parent { get; protected set; }

        /// <summary>
        /// The outer bounds of the current element
        /// </summary>
        public UnitRectangle Outer { get; private set; }

        /// <summary>
        /// The inner bounds of the current element
        /// </summary>
        public UnitRectangle Inner { get; private set; }

        /// <summary>
        /// The element's current state.
        /// </summary>
        public ElementState State { get; private set; }

        /// <summary>
        /// Represents a blacklist of all states unavailable to
        /// the current element.
        /// </summary>
        public ElementState StateBlacklist { get; set; }

        /// <summary>
        /// True if the mouse is directly over the current
        /// elememt.
        /// </summary>
        public Boolean MouseOver { get; private set; }

        /// <summary>
        /// True if the mosue is over every single element contained
        /// within the current elements upward hierarchy
        /// </summary>
        public Boolean MouseOverHierarchy { get; private set; }

        /// <summary>
        /// Mark the current element as dirty
        /// </summary>
        public Boolean Dirty { get; set; }

        /// <summary>
        /// The current elements styleing
        /// </summary>
        public IStyle Style { get; private set; }
        #endregion

        #region Constructors
        public Element(Unit x, Unit y, Unit width, Unit height, Style style = null)
        {
            _children = new List<Element>();
            _vertices = new List<VertexPositionColor>();

            this.layers = new List<Func<SpriteBatch, Rectangle>>();
            this.layers.Add(this.drawBackgroundLayer);

            this.State = ElementState.Normal;
            this.Style = new ElementStyle(this, style);
            this.Outer = new UnitRectangle(x, y, width, height);
            this.Inner = new UnitRectangle(
                x: this.Style.Get<UnitValue>(GlobalProperty.PaddingLeft, 5), 
                y: this.Style.Get<UnitValue>(GlobalProperty.PaddingTop, 5), 
                width: new UnitValue[] { 1f, this.Style.Get<UnitValue>(GlobalProperty.PaddingLeft, 5).Flip(), this.Style.Get<UnitValue>(GlobalProperty.PaddingRight, 5).Flip() }, 
                height: new UnitValue[] { 1f, this.Style.Get<UnitValue>(GlobalProperty.PaddingTop, 5).Flip(), this.Style.Get<UnitValue>(GlobalProperty.PaddingBottom, 5).Flip() }, 
                parent: this.Outer);
            

            this.Dirty = true;

            this.Outer.OnBoundsCleaned += this.handleBoundsChanged;
        }
        #endregion

        #region Frame Methods
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null) // Draw the texture, if there is one
                spriteBatch.Draw(_texture, this.Outer.GlobalBounds, Color.White);

            // Ensure that every self contained child element gets drawn too...
            foreach (Element child in _children)
                child.Draw(spriteBatch);
        }

        public virtual void Update()
        {
            // Update the current element's bounds...
            this.Outer.Update();
            this.Inner.Update();

            // Clean dirty segments of the element
            if (this.Dirty)
            {
                // Add the current element to the dirty texture queue
                this.Stage.dirtyElementQueue.Enqueue(this);

                this.Dirty = false;
            }
                

            // Ensure that every self contained child element gets updated too...
            foreach (Element child in _children)
                child.Update();
        }

        public virtual void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            vertices.AddRange(_vertices);

            // Ensure every child element's debug vertices are added
            foreach (Element child in _children)
                child.AddDebugVertices(ref vertices);
        }

        #endregion

        #region Children Methods
        /// <summary>
        /// Add a new child element to the current element
        /// </summary>
        /// <param name="child"></param>
        protected void add(Element child)
        {
            if (child.Parent != null)
            { // If the child already has a parent...
                throw new Exception($"Unable to add child to element, target already has a parent.");
            }
            else
            { // If the child doesnt already have a parent...
                _children.Add(child);
                child.Parent = this;
                child.Stage = this.Stage;
                child.Outer.setParent(this.Inner);
            }
        }

        /// <summary>
        /// Remove an element from the current parent.
        /// </summary>
        /// <param name="child"></param>
        protected void remove(Element child)
        {
            if(!_children.Contains(child))
            { // If the current element doesnt contain the target...
                throw new Exception($"Unable to remove child from element, target doesn't belong to parent.");
            }
            else
            { // If the current element does contain the target...
                _children.Remove(child);
                child.Parent = null;
                child.Outer.setParent(null);
            }
        }
        #endregion

        #region Utility Methods
        protected Boolean blacklisted(ElementState state)
        {
            return (this.StateBlacklist & state) != 0;
        }

        /// <summary>
        /// Attempt to set the current element state to
        /// the inputed value. If the input is contained
        /// within the blacklist, then no change will 
        /// happen.
        /// </summary>
        /// <param name="state"></param>
        private void setState(ElementState state)
        {
            if(this.State != state && !this.blacklisted(state))
            {
                this.Dirty = true;
            }
        }
        #endregion

        #region Clean Methods
        public void Clean(GraphicsDevice graphicsDevice, RenderTarget2D layerRenderTarget, RenderTarget2D outputRenderTarget, SpriteBatch spriteBatch)
        {
            this.generateDebugVertices();
            this.cleanTexture(graphicsDevice, layerRenderTarget, outputRenderTarget, spriteBatch);
        }

        protected virtual void cleanTexture(GraphicsDevice graphicsDevice, RenderTarget2D layerRenderTarget, RenderTarget2D outputRenderTarget, SpriteBatch spriteBatch)
        {
            if(this.layers.Count > 0)
            {
                // Clear the output, and prep for layer rendering
                graphicsDevice.SetRenderTarget(outputRenderTarget);
                graphicsDevice.Clear(Color.Transparent);

                Rectangle layerPos;

                foreach (Func<SpriteBatch, Rectangle> layer in this.layers)
                { // Iterate through all internal layers
                    graphicsDevice.SetRenderTarget(layerRenderTarget);
                    graphicsDevice.Clear(Color.Transparent);

                    // Generate the specified layer
                    layerPos = layer.Invoke(spriteBatch);

                    // Switch to the output target...
                    graphicsDevice.SetRenderTarget(outputRenderTarget);

                    // Draw the layer target to the output target...
                    spriteBatch.Begin();
                    spriteBatch.Draw(layerRenderTarget, layerPos, layerPos, Color.White);
                    spriteBatch.End();
                }

                
                // Ensure that the current texture is ready for data implantation
                if (_texture == null)
                {
                    _texture = new Texture2D(graphicsDevice, this.Outer.LocalBounds.Width, this.Outer.LocalBounds.Height);
                }
                else if (_texture.Width != this.Outer.LocalBounds.Width || _texture.Height != this.Outer.LocalBounds.Height)
                {
                    _texture?.Dispose();
                    _texture = new Texture2D(graphicsDevice, this.Outer.LocalBounds.Width, this.Outer.LocalBounds.Height);
                }

                // Once the layers are done drawing, convert the output target to a texture
                Color[] textureData = new Color[this.Outer.LocalBounds.Width * this.Outer.LocalBounds.Height];
                outputRenderTarget.GetData<Color>(0, this.Outer.LocalBounds, textureData, 0, textureData.Length);
                _texture.SetData<Color>(textureData);
            }
            else
            {
                // Nothing to draw...
                _texture?.Dispose();
            }
        }
        #endregion

        #region Texture Layer Methods
        private Rectangle drawBackgroundLayer(SpriteBatch spriteBatch)
        {
            var background = this.Style.Get<Texture2D>(this.State, StateProperty.Background);
            
            if(background != null)
            {
                switch (this.Style.Get<DrawType>(this.State, StateProperty.BackgroundType, DrawType.Tile))
                {
                    case DrawType.Normal:
                        spriteBatch.Begin();
                        spriteBatch.Draw(background, this.Outer.LocalBounds.Center.ToVector2() - background.Bounds.Center.ToVector2(), background.Bounds, Color.White);
                        spriteBatch.End();
                        break;
                    case DrawType.Stretch:
                        spriteBatch.Begin();
                        spriteBatch.Draw(background, this.Outer.LocalBounds, Color.White);
                        spriteBatch.End();
                        break;
                    case DrawType.Tile:
                        spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
                        spriteBatch.Draw(background, this.Outer.LocalBounds, this.Outer.LocalBounds, Color.White);
                        spriteBatch.End();
                        break;
                }
            }

            // throw new NotImplementedException();
            return this.Outer.LocalBounds;
        }
        #endregion

        private void generateDebugVertices()
        {
            _vertices.Clear();

            var colorOuter = this.Style.Get<Color>(this.State, StateProperty.OuterDebugColor, Color.Red);
            var colorInner = this.Style.Get<Color>(this.State, StateProperty.InnerDebugColor, Color.Gray);

            // Build inner rectangle
            _vertices.Add(new VertexPositionColor(new Vector3(this.Inner.GlobalBounds.Left, this.Inner.GlobalBounds.Top, 0), colorInner));
            _vertices.Add(new VertexPositionColor(new Vector3(this.Inner.GlobalBounds.Right, this.Inner.GlobalBounds.Top, 0), colorInner));

            _vertices.Add(new VertexPositionColor(new Vector3(this.Inner.GlobalBounds.Right, this.Inner.GlobalBounds.Top, 0), colorInner));
            _vertices.Add(new VertexPositionColor(new Vector3(this.Inner.GlobalBounds.Right, this.Inner.GlobalBounds.Bottom, 0), colorInner));

            _vertices.Add(new VertexPositionColor(new Vector3(this.Inner.GlobalBounds.Right, this.Inner.GlobalBounds.Bottom, 0), colorInner));
            _vertices.Add(new VertexPositionColor(new Vector3(this.Inner.GlobalBounds.Left, this.Inner.GlobalBounds.Bottom, 0), colorInner));

            _vertices.Add(new VertexPositionColor(new Vector3(this.Inner.GlobalBounds.Left, this.Inner.GlobalBounds.Bottom, 0), colorInner));
            _vertices.Add(new VertexPositionColor(new Vector3(this.Inner.GlobalBounds.Left, this.Inner.GlobalBounds.Top, 0), colorInner));

            // Build outer rectangle
            _vertices.Add(new VertexPositionColor(new Vector3(this.Outer.GlobalBounds.Left, this.Outer.GlobalBounds.Top, 0), colorOuter));
            _vertices.Add(new VertexPositionColor(new Vector3(this.Outer.GlobalBounds.Right, this.Outer.GlobalBounds.Top, 0), colorOuter));

            _vertices.Add(new VertexPositionColor(new Vector3(this.Outer.GlobalBounds.Right, this.Outer.GlobalBounds.Top, 0), colorOuter));
            _vertices.Add(new VertexPositionColor(new Vector3(this.Outer.GlobalBounds.Right, this.Outer.GlobalBounds.Bottom, 0), colorOuter));

            _vertices.Add(new VertexPositionColor(new Vector3(this.Outer.GlobalBounds.Right, this.Outer.GlobalBounds.Bottom, 0), colorOuter));
            _vertices.Add(new VertexPositionColor(new Vector3(this.Outer.GlobalBounds.Left, this.Outer.GlobalBounds.Bottom, 0), colorOuter));

            _vertices.Add(new VertexPositionColor(new Vector3(this.Outer.GlobalBounds.Left, this.Outer.GlobalBounds.Bottom, 0), colorOuter));
            _vertices.Add(new VertexPositionColor(new Vector3(this.Outer.GlobalBounds.Left, this.Outer.GlobalBounds.Top, 0), colorOuter));
        }

        private void handleBoundsChanged(object sender, Rectangle e)
        {
            this.Dirty = true;
        }
    }
}
