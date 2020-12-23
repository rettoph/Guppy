using Guppy.DependencyInjection;
using Guppy.UI.Enums;
using Guppy.UI.Extensions.Microsoft.Xna.Framework.Graphics;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Simple <see cref="Element"/> implication capable
    /// of rendering text.
    /// </summary>
    public class TextElement : Element
    {
        #region Private Fields
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        #endregion

        #region Public Properties
        /// <summary>
        /// The color with white text should be rendered.
        /// </summary>
        public ElementStateValue<Color> Color;

        /// <summary>
        /// The current text alignment. Stateless.
        /// </summary>
        public Alignment Alignment;

        /// <summary>
        /// The font to render text. Stateless.
        /// </summary>
        public SpriteFont Font;

        /// <summary>
        /// The text to be rendered. Stateless.
        /// </summary>
        public String Value;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _graphics);
            provider.Service(out _spriteBatch);

            this.Color = this.BuildStateValue<Color>();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _graphics.PushScissorRectangle(this.InnerBounds);

            // TODO: Optimize this possibly?
            // I dont think we need to calculate the alignment every frame.
            _spriteBatch.DrawString(
                this.Font,
                this.Value,
                this.Alignment.Align(this.Font.MeasureString(this.Value), this.InnerBounds),
                this.Color);

            _graphics.PopScissorRectangle();
        }
        #endregion
    }
}
