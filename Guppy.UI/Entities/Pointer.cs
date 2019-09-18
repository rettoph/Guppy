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
        public Single Scroll { get; private set; }
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
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.DrawString(_font, $"Pointer: {this.Position.X}, {this.Position.Y}", new Vector2(15, 15), Color.White);
        }
        #endregion

        #region Utility Methods
        public void MoveTo(Vector2 position)
        {
            this.Position = position;

            this.Events.TryInvoke<Vector2>(this, "moved", this.Position);
        }

        public void MoveBy(Vector2 delta)
        {
            this.MoveTo(this.Position + delta);
        }

        public void ScrollTo(Single scroll)
        {
            this.Scroll = scroll;

            this.Events.TryInvoke<Single>(this, "scrolled", this.Scroll);
        }

        public void ScrollBy(Single delta)
        {
            this.ScrollTo(this.Scroll + delta);
        }

        public void SetButton(Button button, Boolean value)
        {
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
        #endregion
    }
}
