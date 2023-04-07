using Guppy.GUI.Elements;
using Guppy.GUI.Providers;
using Guppy.MonoGame.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public class Stage : Container<Element>
    {
        private static readonly Point PointX = new Point(1, 0);
        private static readonly Point PointOne = new Point(1, 1);

        private readonly IStyleSheetProvider _styles;
        private readonly IScreen _screen;

        public IStyleSheet StyleSheet { get; private set; }
        public readonly SpriteBatch SpriteBatch;
        public readonly PrimitiveBatch<VertexPositionColor> PrimitiveBatch;

        public Stage(
            GraphicsDevice graphics,
            IScreen screen,
            IStyleSheetProvider styles)
        {
            _styles = styles;
            _screen = screen;

            this.StyleSheet = null!;
            this.SpriteBatch = new SpriteBatch(graphics);
            this.PrimitiveBatch = new PrimitiveBatch<VertexPositionColor>(graphics);

            _screen.Window.ClientSizeChanged += this.HandleClientSizeChanged;
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
            this.PrimitiveBatch.Begin(_screen.Camera);

            base.Draw(gameTime, PointX);

            this.PrimitiveBatch.End();
        }

        public new void Add(Element element)
        {
            base.Add(element);
        }

        public new void Remove(Element element)
        {
            base.Remove(element);
        }

        protected override Rectangle GetConstraints()
        {
            return new Rectangle()
            {
                X = 0,
                Y = 0,
                Width = _screen.Window.ClientBounds.Width - 1,
                Height = _screen.Window.ClientBounds.Height - 1,
            };
        }
    }
}
