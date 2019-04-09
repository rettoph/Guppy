using Guppy.UI.Elements.ElementSegments;
using Guppy.UI.Entities;
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
using System.Threading;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Element is the base type that all
    /// UI elements derive from.
    /// </summary>
    public abstract class Element
    {
        #region Private Attributes
        private Boolean _mouseWasRaised;
        #endregion

        #region Protected Fields
        protected internal Stage stage;
        #endregion


        #region Public Fields
        public readonly Style Style;
        #endregion

        #region Public Attributes
        /// <summary>
        /// Manages the inner bounds/textures of the current
        /// element
        /// </summary>
        public InnerElementSegment Inner { get; private set; }
        /// <summary>
        /// Manages the inner bounds/textures of the
        /// current element
        /// </summary>
        public OuterElementSegment Outer { get; private set; }

        /// <summary>
        /// Mark the elements meta data as dirty.
        /// This will mark self container meta data
        /// such as older & younger sibling as dirty
        /// and ensure it is reloaded next update call
        /// </summary>
        public Boolean DirtyMeta { get; set; }

        /// <summary>
        /// Mark the elements texture as dirty.
        /// This will trigger an async texture regeneration
        /// next update.
        /// </summary>
        public Boolean DirtyTextures { get; set; }

        /// <summary>
        /// The current non blacklisted element state
        /// </summary>
        public ElementState State { get; private set; }

        /// <summary>
        /// Specific element states can be blacklisted here.
        /// The element state will never be set to any values
        /// within the blacklist
        /// </summary>
        public ElementState StateBlacklist { get; set; }

        /// <summary>
        /// Indicates if the mouse was over the current element on 
        /// the last frame update
        /// </summary>
        public Boolean MouseOver { get; private set; }

        /// <summary>
        /// Indicates if the mouse is over all elements
        /// in the current elements upward heierarchy
        /// </summary>
        public Boolean MouseOverHierarchy { get; private set; }

        public Container Container { get; private set; }

        public Element PrevSibling { get; private set; }
        public Element NextSibling { get; private set; }
        #endregion

        #region Events
        public event EventHandler<Element> OnStateChanged;
        #endregion


        #region Constructors
        public Element(Style style = null)
        {
            this.Style = new Style(style);
            // Create the internal element segments
            this.Outer = new OuterElementSegment(this);
            this.Inner = new InnerElementSegment(this);
            this.Inner.setParent(this.Outer);

            // Update Inner Bounds
            this.Inner.X.SetValue(this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingLeft));
            this.Inner.Y.SetValue(this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingTop));
            this.Inner.Width.SetValue(new UnitValue[] { 1f, this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingRight).Flip(), this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingLeft).Flip() });
            this.Inner.Height.SetValue(new UnitValue[] { 1f, this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingTop).Flip(), this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingBottom).Flip() });

            this.State = ElementState.Normal;
            this.DirtyTextures = true;
        }
        public Element(UnitValue x, UnitValue y, UnitValue width, UnitValue height, Style style = null)
        {
            this.Style = new Style(style);

            // Create the internal element segments
            this.Outer = new OuterElementSegment(this);
            this.Inner = new InnerElementSegment(this);
            this.Inner.setParent(this.Outer);

            // Update Outer Bounds
            this.Outer.X.SetValue(x);
            this.Outer.Y.SetValue(y);
            this.Outer.Width.SetValue(width);
            this.Outer.Height.SetValue(height);

            // Update Inner Bounds
            this.Inner.X.SetValue(this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingLeft));
            this.Inner.Y.SetValue(this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingTop));
            this.Inner.Width.SetValue(new UnitValue[] { 1f, this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingRight).Flip(), this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingLeft).Flip() });
            this.Inner.Height.SetValue(new UnitValue[] { 1f, this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingTop).Flip(), this.Style.Get<UnitValue>(GlobalStyleProperty.PaddingBottom).Flip() });

            this.State = ElementState.Normal;
            this.DirtyTextures = true;
        }
        #endregion

        #region Frame Methods
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.Outer.Draw(spriteBatch);
            this.Inner.Draw(spriteBatch);
        }

        public virtual void Update(GameTime gameTime)
        {
            // Ensure the inner and outer bounds are updated
            this.Outer.Update();
            this.Inner.Update();

            // Update the current mouse over positions
            this.MouseOver = this.Outer.Bounds.Contains(this.stage.inputManager.Position);
            this.MouseOverHierarchy = this.Container == null ? true : this.MouseOver && this.Container.MouseOverHierarchy;

            if(this.MouseOverHierarchy)
            { // Mouse is over element...
                if(this.stage.inputManager.Down)
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
                if(this.stage.inputManager.Down)
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
        }
        #endregion

        #region Cleaning Methods
        protected virtual void CleanMeta()
        {
            if(this.Container == null)
            {
                this.PrevSibling = null;
                this.NextSibling = null;
            }
            else
            {
                var index = this.Container.Children.IndexOf(this);
                this.PrevSibling = index > 0 ? this.Container.Children[index - 1] : null;
                this.NextSibling = this.Container.Children.Count > index + 1 ? this.Container.Children[index + 1] : null;
            }
        }
        #endregion

        #region Set Methods
        protected internal void setContainer(Container container)
        {
            // Save the new parent
            this.Container = container;
            this.stage = container.stage;

            this.Outer.setParent(this.Container.Inner);

            // Mark the element as dirty
            this.DirtyMeta = true;
        }

        protected void setState(params ElementState[] states)
        {
            foreach(ElementState newState in states)
            {
                if (newState != this.State && !this.StateBlacklisted(newState))
                { // If the new state is not the current public state and is not blacklisted...
                    // Set the new state
                    this.State = newState;

                    // Trigger the on update event
                    this.OnStateChanged?.Invoke(this, this);

                    break;
                }
            }

        }
        #endregion

        #region Generation Methods
        protected virtual void generateTexture(ElementState state, SpriteBatch spriteBatch, RenderTarget2D renderTarget, GraphicsDevice graphicsDevice)
        {

        }
        #endregion

        public virtual void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            this.Outer.AddDebugVertices(ref vertices);
            this.Inner.AddDebugVertices(ref vertices);
        }

        #region Helper Methods
        protected internal Boolean StateBlacklisted(ElementState state)
        {
            return (state & this.StateBlacklist) != 0;
        }
        #endregion
    }
}
