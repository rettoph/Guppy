using Guppy.Common;
using Guppy.GUI.Elements;
using Guppy.GUI.Providers;
using Guppy.Input;
using Guppy.Input.Constants;
using Guppy.Input.Messages;
using Guppy.Input.Providers;
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
    public class Stage : ScrollContainer<Element>
    {
        private readonly IStyleSheetProvider _styles;
        private readonly RasterizerState _rasterizerState;
        public IStyleSheet StyleSheet { get; private set; }
        public readonly SpriteBatch SpriteBatch;
        public readonly PrimitiveBatch<VertexPositionColor> PrimitiveBatch;
        public readonly IScreen Screen;
        public readonly IBus Bus;
        public readonly ICursor Mouse;
        public readonly Texture2D Pixel;

        public Stage(
            GraphicsDevice graphics,
            IScreen screen,
            IStyleSheetProvider styles,
            ICursorProvider cursors,
            IBus bus)
        {
            _styles = styles;
            _rasterizerState = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                ScissorTestEnable = true,
            };

            this.StyleSheet = null!;
            this.SpriteBatch = new SpriteBatch(graphics);
            this.PrimitiveBatch = new PrimitiveBatch<VertexPositionColor>(graphics);
            this.Screen = screen;
            this.Bus = bus;
            this.Mouse = cursors.Get(Cursors.Mouse);
            this.Pixel = this.Screen.Graphics.BuildPixel();

            this.Screen.Window.ClientSizeChanged += this.HandleClientSizeChanged;
            this.PrimitiveBatch.OnEarlyFlush += this.HandleEarlyPrimitiveFlush;
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

        protected internal override void Uninitialize()
        {
            base.Uninitialize();
        }

        public void Draw(GameTime gameTime)
        {
            this.Screen.Graphics.ScissorRectangle = this.Screen.Graphics.Viewport.Bounds;

            this.SpriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                rasterizerState: _rasterizerState);

            this.PrimitiveBatch.Begin(this.Screen.Camera);

            this.TryDraw(gameTime, Vector2.UnitX);

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

        private void HandleClientSizeChanged(object? sender, EventArgs e)
        {
            this.Clean();
        }

        private void HandleEarlyPrimitiveFlush(PrimitiveBatch<VertexPositionColor> args)
        {
            this.SpriteBatch.End();

            this.SpriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                rasterizerState: _rasterizerState);
        }

        public void Process(in CursorMove message)
        {

        }
    }
}
