using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.UI.Extensions;
using Guppy.UI.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// The main element class. This defines basic
    /// sizing, debug rendering, child relationships,
    /// & more.
    /// </summary>
    public class BaseElement : Entity
    {
        #region Enums
        public enum EventTypes {
            /// <summary>
            /// Track no events on this element
            /// </summary>
            None,
            /// <summary>
            /// Only track top level events on this element
            /// </summary>
            Normal,
            /// <summary>
            /// Track all events on this element no matter its position
            /// </summary>
            Propagate,
        }

        [Flags]
        public enum Alignment
        {
            Top = 1,
            VerticalCenter = 2,
            Bottom = 4,
            Left = 8,
            HorizontalCenter = 16,
            Right = 32,

            TopLeft = Alignment.Top | Alignment.Left,
            TopCenter = Alignment.Top | Alignment.HorizontalCenter,
            TopRight = Alignment.Top | Alignment.Right,

            CenterLeft = Alignment.VerticalCenter | Alignment.Left,
            CenterCenter = Alignment.VerticalCenter | Alignment.HorizontalCenter,
            Center = Alignment.VerticalCenter | Alignment.HorizontalCenter,
            CenterRight = Alignment.VerticalCenter | Alignment.Right,

            BottomLeft = Alignment.Bottom | Alignment.Left,
            BottomCenter = Alignment.Bottom | Alignment.HorizontalCenter,
            BottomRight = Alignment.Bottom | Alignment.Right
        }
        #endregion

        #region Private Fields
        private HashSet<Element> _children;
        #endregion

        #region Protected Attributes
        /// <summary>
        /// The scoped UI pointer instance.
        /// </summary>
        protected Pointer pointer { get; private set; }

        /// <summary>
        /// A collection of all entities (mainly useful for creating new entities)
        /// </summary>
        protected EntityCollection entities { get; private set; }

        /// <summary>
        /// If the current entity is dirty, it will be cleaned
        /// during its next update.
        /// </summary>
        protected Boolean dirty { get; set; }

        /// <summary>
        /// List of all children currently contained within the element
        /// </summary>
        protected IReadOnlyCollection<Element> children => _children;

        /// <summary>
        /// The current element's root stage.
        /// </summary>
        protected virtual Stage stage => this.Parent.stage;

        /// <summary>
        /// Indicates that a child element has been hovered this frame,
        /// effectively preventing event propagation when desired.
        /// </summary>
        protected internal Boolean childHovered { get; set; }

        protected virtual PrimitiveBatch primitiveBatch { get => this.stage.primitiveBatch; }

        protected virtual SpriteBatch spriteBatch { get => this.stage.spriteBatch; }
        #endregion

        #region Public Attributes
        /// <summary>
        /// The current bounds, as of the most recent cleaning.
        /// </summary>
        public ElementBounds Bounds { get; protected set; }

        /// <summary>
        /// The current element's parent (if any)
        /// </summary>
        public BaseElement Parent { get; internal set; }

        /// <summary>
        /// Defines if the pointer is currently over the element
        /// </summary>
        public Boolean Hovered { get; private set; }

        /// <summary>
        /// The buttons currently pressed over the element
        /// </summary>
        public Pointer.Button Buttons { get; private set; }
        
        /// <summary>
        /// The elements current event tracking type.
        /// </summary>
        public EventTypes EventType { get; set; } = EventTypes.Normal;
        #endregion

        #region Event 
        public event EventHandler<Boolean> OnHoveredChanged;
        public event EventHandler<Pointer.Button> OnButtonPressed;
        public event EventHandler<Pointer.Button> OnButtonReleased;
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _children = new HashSet<Element>();

            this.entities = provider.GetRequiredService<EntityCollection>();
            this.pointer = provider.GetRequiredService<Pointer>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.dirty = true;

            this.SetEnabled(false);
            this.SetVisible(false);

            this.Bounds = new ElementBounds(this);
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.Bounds.OnChanged += this.HandleBoundsChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Bounds.OnChanged -= this.HandleBoundsChanged;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.TryClean();

            // Bit used to track if any internal elements are getting hovered, as only the top most element shoudl trigger click events.
            this.childHovered = false;

            // Update all internal children...
            this.children.TryUpdateAll(gameTime);

            if (this.EventType != EventTypes.None)
            {
                // Update the current over status & trigger an event if needed
                if (this.Hovered != (this.Hovered = this.GetHovered()))
                    this.OnHoveredChanged?.Invoke(this, this.Hovered);

                // Update all the pointer buttons
                this.UpdateButton(Pointer.Button.Left);
                this.UpdateButton(Pointer.Button.Middle);
                this.UpdateButton(Pointer.Button.Right);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            Color color = new Color(100, 100, 0);

            if (this.Buttons.HasFlag(Pointer.Button.Left))
                color.R = 255;
            if (this.Buttons.HasFlag(Pointer.Button.Right))
                color.G = 255;
            if (this.Hovered)
                color.B = 255;

            this.primitiveBatch.FillRectangle(this.Align(new Rectangle(0, 0, 100, 100), Alignment.Center, true), color);

            this.primitiveBatch.DrawRectangle(this.Bounds, color);

            this.children.TryDrawAll(gameTime);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Attempt to clean the current element.
        /// This will also call clean on all internal
        /// children (if any)
        /// </summary>
        /// <param name="force"></param>
        public void TryClean(Boolean force = false)
        {
            if(this.dirty || force)
            {
                this.Clean();
                this.dirty = false;
            }
        }

        /// <summary>
        /// This should be the real brains behind the public 
        /// TryClean method
        /// </summary>
        protected virtual void Clean()
        {
            this.Bounds.Clean();
            this.children.ForEach(c => c.dirty = true);
        }

        public void Add(Element child)
        {
            if(_children.Add(child))
            {
                child.Parent = this;
                child.dirty = true;
            }
        }
        public void Add<T>(String handle = default(String), Action<T> setup = null, Action<T> create = null)
            where T : Element
        {
            this.Add(this.entities.Create<T>(handle, setup, create));
        }
        public void Add<T>(Action<T> setup = null, Action<T> create = null)
            where T : Element
        {
            this.Add(this.entities.Create<T>(setup, create));
        }

        public void Remove(Element child)
        {
            _children.Remove(child);
        }

        protected internal virtual Rectangle GetParentBounds()
        {
            return this.Parent.Bounds;
        }

        private void UpdateButton(Pointer.Button button)
        {
            if (this.CanTriggerEvents() && this.Hovered && (this.stage.pressed & ~this.Buttons & button) != 0)
            {
                this.Buttons |= button;
                this.OnButtonPressed?.Invoke(this, button);
            }
            else if ((this.stage.released & this.Buttons & button) != 0)
            {
                this.Buttons &= ~button;
                this.OnButtonReleased?.Invoke(this, button);
            }
        }

        /// <summary>
        /// Calculate the current elements hovered state.
        /// </summary>
        /// <returns></returns>
        protected virtual Boolean GetHovered()
        {
            return this.CanTriggerEvents() && this.Bounds.Contains(this.pointer.Position);
        }

        /// <summary>
        /// Internal helper class designed to help detect
        /// if the current element should trigger top level events
        /// </summary>
        /// <returns></returns>
        private Boolean CanTriggerEvents()
        {
            return this.EventType == EventTypes.Propagate || (this.EventType == EventTypes.Normal && !this.childHovered);
        }

        
        /// <summary>
        /// Aligns the given rectangle in the requested alignment method.
        /// 
        /// This will update the rectangles internal position.
        /// </summary>
        /// <param name="rectangle">The rectangle to align.</param>
        /// <param name="alignment">The requested alignment type. Default is Top Left</param>
        /// <param name="useWorldCoordinates">Whether or not the response should be in local or world coords.</param>
        public void Align(ref Rectangle rectangle, BaseElement.Alignment alignment, Boolean useWorldCoordinates = false)
        {
            // Default to Top Left alignment...
            Point position = useWorldCoordinates ? this.Bounds.Pixel.Location : Point.Zero;

            // Vertical Alignment...
            if((alignment & Alignment.Bottom) != 0)
            { // Bottom align...
                position.Y += this.Bounds.Pixel.Height - rectangle.Height;
            }
            else if ((alignment & Alignment.VerticalCenter) != 0)
            { // VerticalCenter align...
                position.Y += (this.Bounds.Pixel.Height - rectangle.Height) / 2;
            }

            // Horizontal Alignment
            if ((alignment & Alignment.Right) != 0)
            { // Right align...
                position.X += this.Bounds.Pixel.Width - rectangle.Width;
            }
            else if ((alignment & Alignment.HorizontalCenter) != 0)
            { // HorizontalCenter align...
                position.X += (this.Bounds.Pixel.Width - rectangle.Width) / 2;
            }

            // Update the recieved rectangles position.
            rectangle.Location = position;
        }
        /// <summary>
        /// Returns a rectangle aligned to the current element with the
        /// requested alignment type
        /// </summary>
        /// <param name="rectangle">The rectangle to align.</param>
        /// <param name="alignment">The requested alignment type. Default is Top Left</param>
        /// <param name="useWorldCoordinates">Whether or not the response should be in local or world coords.</param>
        public Rectangle Align(Rectangle rectangle, BaseElement.Alignment alignment, Boolean useWorldCoordinates = false)
        {
            this.Align(ref rectangle, alignment, useWorldCoordinates);
            return rectangle;
        }
        #endregion

        #region Event Handlers
        private void HandleBoundsChanged(object sender, EventArgs e)
        {
            this.dirty = true;
        }
        #endregion
    }
}
