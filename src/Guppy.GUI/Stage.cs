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
    public class Stage : Container<Element>
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

            this.Screen = screen;
            this.StyleSheet = null!;
            this.SpriteBatch = new SpriteBatch(this.Screen.Graphics);
            this.PrimitiveBatch = new PrimitiveBatch<VertexPositionColor>(this.Screen.Graphics);
            this.Bus = bus;
            this.Mouse = cursors.Get(Cursors.Mouse);
            this.Pixel = this.Screen.Graphics.BuildPixel();

            this.Screen.Window.ClientSizeChanged += this.HandleClientSizeChanged;
            this.PrimitiveBatch.OnEarlyFlush += this.HandleEarlyPrimitiveFlush;
        }

        public void Initialize(IStyleSheet styleSheet, params string[] names)
        {
            if (names.Length > 0)
            {
                this.Selector.Names.AddRange(names);
            }

            this.StyleSheet = styleSheet;            
            base.Initialize(this, null);
            this.Clean();
        }

        public void Initialize(string styleSheetName, params string[] names)
        {
            var styles = _styles.Get(styleSheetName);

            this.Initialize(styles, names);
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

        protected override RectangleF GetOuterConstraints()
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
