using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Boolean _mouseWasRaised;

        

        private List<VertexPositionColor> _vertices;


        #endregion

        #region Protected Fields
        protected Texture2D texture;

        /// <summary>
        /// Contains layers to be drawn within the current element.
        /// Custom layers can be added at anytime, and next time the
        /// texture is cleaned, the layer will be ran then appended
        /// to the input texture.
        /// </summary>
        protected List<Func<SpriteBatch, Rectangle>> layers;

        protected List<Element> children;
        #endregion

        #region Public Attributes
        public virtual Stage Stage { get { return this.Parent?.Stage; } }

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
            this.children = new List<Element>();
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
            if (texture != null && this.Outer.GlobalBounds.Intersects(this.Stage.clientBounds.GlobalBounds)) // Draw the texture, if there is one
                spriteBatch.Draw(texture, this.Outer.GlobalBounds, Color.White);

            // Ensure that every self contained child element gets drawn too...
            foreach (Element child in this.children)
                child.Draw(spriteBatch);
        }

        public virtual void Update(MouseState mouse)
        {
            // Update the current element's bounds...
            this.Outer.Update();
            this.Inner.Update();

            // Update the mouse over data
            this.MouseOver = this.Outer.GlobalBounds.Contains(mouse.Position);
            this.MouseOverHierarchy = this.MouseOver && (this.Parent == null ? true : this.Parent.MouseOverHierarchy);

            if (this.MouseOverHierarchy)
            { // Mouse is over element...
                if (mouse.LeftButton == ButtonState.Pressed)
                { // If mouse is down
                    switch (this.State)
                    {
                        case ElementState.Normal:
                            _mouseWasRaised = false;
                            this.setState(ElementState.Hovered, ElementState.Normal);
                            break;
                        case ElementState.Hovered:
                            if (_mouseWasRaised)
                                this.setState(ElementState.Pressed, ElementState.Hovered);
                            break;
                        case ElementState.Active:
                            break;
                        case ElementState.Pressed:
                            break;
                    }
                }
                else
                { // If mouse is up...
                    switch (this.State)
                    {
                        case ElementState.Normal:
                            _mouseWasRaised = true;
                            this.setState(ElementState.Hovered, ElementState.Normal);
                            break;
                        case ElementState.Hovered:
                            break;
                        case ElementState.Active:
                            break;
                        case ElementState.Pressed:
                            this.setState(ElementState.Active, ElementState.Hovered, ElementState.Normal);
                            break;
                    }
                }
            }
            else
            { // Mouse is not over element...
                if (mouse.LeftButton == ButtonState.Pressed)
                { // If mouse is down
                    switch (this.State)
                    {
                        case ElementState.Normal:
                            break;
                        case ElementState.Hovered:
                            this.setState(ElementState.Normal);
                            break;
                        case ElementState.Active:
                            this.setState(ElementState.Normal);
                            break;
                        case ElementState.Pressed:
                            this.setState(ElementState.Normal);
                            break;
                    }
                }
                else
                { // If mouse is up...
                    switch (this.State)
                    {
                        case ElementState.Normal:
                            break;
                        case ElementState.Hovered:
                            this.setState(ElementState.Normal);
                            break;
                        case ElementState.Active:
                            break;
                        case ElementState.Pressed:
                            this.setState(ElementState.Normal);
                            break;
                    }
                }
            }


            // Clean dirty segments of the element
            if (this.Dirty)
            {
                // Add the current element to the dirty texture queue
                this.Stage.dirtyElementQueue.Enqueue(this);

                this.Dirty = false;
            }


            // Ensure that every self contained child element gets updated too...
            foreach (Element child in this.children)
                child.Update(mouse);
        }

        public virtual void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            vertices.AddRange(_vertices);

            // Ensure every child element's debug vertices are added
            foreach (Element child in this.children)
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
                this.children.Add(child);
                child.Parent = this;
                child.Outer.setParent(this.Inner);
            }
        }

        /// <summary>
        /// Remove an element from the current parent.
        /// </summary>
        /// <param name="child"></param>
        protected void remove(Element child)
        {
            if(!this.children.Contains(child))
            { // If the current element doesnt contain the target...
                throw new Exception($"Unable to remove child from element, target doesn't belong to parent.");
            }
            else
            { // If the current element does contain the target...
                this.children.Remove(child);
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
        /// <param name="states"></param>
        protected void setState(params ElementState[] states)
        {
            foreach (ElementState newState in states)
            {
                if (newState != this.State && !this.blacklisted(newState))
                { // If the new state is not the current public state and is not blacklisted...
                    // Set the new state
                    this.State = newState;

                    this.Dirty = true;

                    break;
                }
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
                Boolean empty = true;
                // Color[] layerData;

                foreach (Func<SpriteBatch, Rectangle> layer in this.layers)
                { // Iterate through all internal layers
                    graphicsDevice.SetRenderTarget(layerRenderTarget);
                    graphicsDevice.Clear(Color.Transparent);

                    // Generate the specified layer
                    layerPos = layer.Invoke(spriteBatch);

                    if (!layerPos.Equals(Rectangle.Empty))
                    { // If the returned rectangle is not empty...
                        empty = false; // We know for a fact that there is at least one layer with data...

                        // Switch to the output target...
                        graphicsDevice.SetRenderTarget(outputRenderTarget);

                        // Draw the layer target to the output target...
                        spriteBatch.Begin(blendState: BlendState.AlphaBlend);
                        spriteBatch.Draw(layerRenderTarget, layerPos, layerPos, Color.White);
                        spriteBatch.End();
                    }
                }


                if (empty)
                { // Empty layers.. nothing to draw...
                    this.texture?.Dispose();
                }
                else
                {
                    var overlap = outputRenderTarget.Bounds.Overlap(this.Outer.LocalBounds);

                    if (overlap.X >= 0 && overlap.Y >= 0 && overlap.Width > 0 && overlap.Height > 0)
                    {
                        // Ensure that the current texture is ready for data implantation
                        if (this.texture == null)
                        {
                            this.texture = new Texture2D(graphicsDevice, overlap.Width, overlap.Height);
                        }
                        else if (this.texture.Width != overlap.Width || this.texture.Height != overlap.Height)
                        {
                            this.texture?.Dispose();
                            this.texture = new Texture2D(graphicsDevice, overlap.Width, overlap.Height);
                        }

                        // Once the layers are done drawing, convert the output target to a texture
                        Color[] textureData = new Color[overlap.Width * overlap.Height];
                        outputRenderTarget.GetData<Color>(0, overlap, textureData, 0, textureData.Length);
                        this.texture.SetData<Color>(textureData);
                        textureData = null;
                    }
                    else
                    { // The texture has no overlap bounds...
                        this.texture?.Dispose();
                    }
                }
            }
            else
            {
                // Nothing to draw...
                this.texture?.Dispose();
            }
        }
        #endregion

        #region Texture Layer Methods
        /// <summary>
        /// Draw the current elements texture layer,
        /// if any is defined...
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <returns></returns>
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

                return this.Outer.LocalBounds;
            }

            // No background, so just return empty bounds
            return Rectangle.Empty;
        }
        #endregion

        #region Generate Methods
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
        #endregion

        #region Event Handlers
        private void handleBoundsChanged(object sender, Rectangle e)
        {
            this.Dirty = true;
        }
        #endregion
    }
}
