using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities
{
    /// <summary>
    /// Represents a simple interface that can control a
    /// position, button presses, and scroll actions.
    /// 
    /// This should be used to access mouse data
    /// </summary>
    public class Pointer : Entity
    {
        #region Enums
        [Flags]
        public enum Button
        {
            Left = 1,
            Middle = 2,
            Right = 4
        }
        #endregion

        #region Private Fields
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The pointers current position
        /// in screen coordinates
        /// </summary>
        public Vector2 Position { get; private set; }
        /// <summary>
        /// The buttons currently pressed.
        /// </summary>
        public Button Buttons { get; private set; }

        /// <summary>
        /// The pointers current scroll value
        /// </summary>
        public Int32 Scroll { get; private set; }
        #endregion

        #region Constructor
        public Pointer(SpriteBatch spriteBatch, ContentManager content)
        {
            _font = content.Load<SpriteFont>("font");
            _spriteBatch = spriteBatch;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Events.Register<Vector2>("moved");
            this.Events.Register<Button>("pressed");
            this.Events.Register<Button>("released");
            this.Events.Register<Single>("scrolled");
        }
        #endregion

        #region Frame Methods
        #endregion

        #region Utility Methods
        public void MoveTo(Vector2 position)
        {
            if (position != this.Position)
            { // Only update the position if anything has changed
                this.Position = position;

                this.Events.TryInvoke<Vector2>(this, "moved", this.Position);
            }
        }

        public void MoveBy(Vector2 delta)
        {
            this.MoveTo(this.Position + delta);
        }

        public void ScrollTo(Int32 scroll)
        {
            if (scroll != this.Scroll)
            { // Only scroll if anything has changed
                this.Scroll = scroll;

                this.Events.TryInvoke<Int32>(this, "scrolled", this.Scroll);
            }
        }

        public void ScrollBy(Int32 delta)
        {
            this.ScrollTo(this.Scroll + delta);
        }

        public void SetButton(Button button, Boolean value)
        {
            if (value != this.Buttons.HasFlag(button))
            { // Only update if the button value is not already the recieved value
                if (value)
                {
                    this.Buttons |= button;
                    this.Events.TryInvoke<Button>(this, "pressed", button);
                }
                else
                {
                    this.Buttons &= ~button;
                    this.Events.TryInvoke<Button>(this, "released", button);
                }
            }
        }
        #endregion
    }
}
