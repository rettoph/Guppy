using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities
{
    public sealed class Cursor : Entity
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
        /// The buttons that were just pressed within the last frame.
        /// </summary>
        public Button Pressed { get; private set; }

        /// <summary>
        /// The buttons that were just released within the last frame.
        /// </summary>
        public Button Released { get; private set; }

        /// <summary>
        /// The pointers current scroll value
        /// </summary>
        public Int32 Scroll { get; private set; }
        #endregion

        #region Constructor
        public Cursor()
        {
        }
        #endregion

        #region Events
        public event GuppyDeltaEventHandler<Cursor, Vector2> OnMoved;
        public event GuppyEventHandler<Cursor, Button> OnPressed;
        public event GuppyEventHandler<Cursor, Button> OnReleased;
        public event GuppyDeltaEventHandler<Cursor, Single> OnScrolled;
        #endregion

        #region Frame Methods
        protected override void PreUpdate(GameTime gameTIme)
        {
            base.PreUpdate(gameTIme);

            this.Pressed = 0;
            this.Released = 0;
        }
        #endregion

        #region Utility Methods
        public void MoveTo(Vector2 position)
        {
            if (position != this.Position)
            { // Only update the position if anything has changed
                var old = this.Position;
                this.Position = position;

                this.OnMoved?.Invoke(this, old, this.Position);
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
                var old = this.Scroll;
                this.Scroll = scroll;

                this.OnScrolled?.Invoke(this, old, this.Scroll);
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
                    this.Pressed |= button;
                    this.OnPressed?.Invoke(this, button);
                }
                else
                {
                    this.Buttons &= ~button;
                    this.Released |= button;
                    this.OnReleased?.Invoke(this, button);
                }
            }
        }
        #endregion
    }
}
