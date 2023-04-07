using Guppy.GUI.Elements;
using Guppy.GUI.Providers;
using Guppy.MonoGame.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public class Stage : Container<Element>
    {
        private readonly IStyleSheetProvider _styles;
        private readonly RasterizerState _raster;
        public IStyleSheet StyleSheet { get; private set; }
        public readonly SpriteBatch SpriteBatch;
        public readonly PrimitiveBatch<VertexPositionColor> PrimitiveBatch;
        public readonly IScreen Screen;

        public Stage(
            GraphicsDevice graphics,
            IScreen screen,
            IStyleSheetProvider styles)
        {
            _styles = styles;
            _raster = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                ScissorTestEnable = true,
            };

            this.StyleSheet = null!;
            this.SpriteBatch = new SpriteBatch(graphics);
            this.PrimitiveBatch = new PrimitiveBatch<VertexPositionColor>(graphics);
            this.Screen = screen;

            this.Screen.Window.ClientSizeChanged += this.HandleClientSizeChanged;
        }

        private void HandleClientSizeChanged(object? sender, EventArgs e)
        {
            this.Clean();
        }

        public void Initialize(IStyleSheet styleSheet)
        {
            this.StyleSheet = styleSheet;
            this.Initialize(this, null);
            this.Clean();
        }

        public void Initialize(string styleSheetName)
        {
            var styles = _styles.Get(styleSheetName);

            this.Initialize(styles);
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            this.Screen.Graphics.ScissorRectangle = this.Screen.Graphics.Viewport.Bounds;

            this.SpriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                rasterizerState: _raster);

            this.PrimitiveBatch.Begin(this.Screen.Camera);

            base.Draw(gameTime, Vector2.UnitX);

            this.PrimitiveBatch.End();
            this.SpriteBatch.End();
        }

        public new void Add(Element element)
        {
            base.Add(element);
        }

        public new void Remove(Element element)
        {
            base.Remove(element);
        }

        protected override RectangleF GetConstraints()
        {
            return new RectangleF()
            {
                X = 0,
                Y = 0,
                Width = this.Screen.Window.ClientBounds.Width - 1,
                Height = this.Screen.Window.ClientBounds.Height - 1,
            };
        }
    }
}
