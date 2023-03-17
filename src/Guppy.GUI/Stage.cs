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
            this.Clean(out _);
        }

        public void Initialize(IStyleSheet styleSheet)
        {
            this.StyleSheet = styleSheet;
            this.Initialize(this, null);
            this.Clean(out _);
        }

        public void Initialize(string styleSheetName)
        {
            var styles = _styles.Get(styleSheetName);

            this.Initialize(styles);
        }

        public void Draw(GameTime gameTime)
        {
            this.PrimitiveBatch.Begin(_screen.Camera);

            base.Draw(gameTime, new Point(1, 0));

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

        protected override Point GetSizeConstraints()
        {
            return _screen.Window.ClientBounds.Size - new Point(1, 1);
        }
    }
}
