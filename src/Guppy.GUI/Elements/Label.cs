using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class Label : Element
    {
        private IStyle<SpriteFont> _font = null!;
        private IStyle<Color> _color = null!;

        public string Text = string.Empty;

        protected internal override void Initialize(Stage stage, Element? parent)
        {
            base.Initialize(stage, parent);

            _font = this.stage.StyleSheet.Get<SpriteFont>(Property.Font, this);
            _color = this.stage.StyleSheet.Get<Color>(Property.Color, this);
        }

        protected override void DrawContent(GameTime gameTime, Point position)
        {
            base.DrawContent(gameTime, position);
        }
    }
}
