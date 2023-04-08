using Guppy.GUI.Extensions.Microsoft.Xna.Framework;
using Guppy.GUI.Extensions.System.Drawing;
using Guppy.MonoGame.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.GUI.Elements
{
    public class Element
    {
        private static PrimitiveShape _shape = new PrimitiveShape(new Vector3[5]);

        private RectangleF _outerBounds;
        private RectangleF _innerBounds;
        private RectangleF _contentBounds;
        private Vector2 _contentAlignment;
        private IStyle<bool> _inline = null!;
        private IStyle<Unit> _width = null!;
        private IStyle<Unit> _height = null!;
        private IStyle<Padding> _padding = null!;
        private IStyle<Alignment> _alignment = null!;

        protected Stage stage;
        protected Element? parent;
        protected ElementState state;

        public Selector Selector { get; }
        public ElementState State => this.state;

        public RectangleF OuterBounds => _outerBounds;
        public RectangleF InnerBounds => _innerBounds;
        public RectangleF ContentBounds => _contentBounds;

        public bool Inline => _inline.GetValue(this.state);
        public Unit? Width => _width.GetValue(this.state);
        public Unit? Height => _height.GetValue(this.state);
        public Padding? Padding => _padding.GetValue(this.state);
        public Alignment Alignment => _alignment.GetValue(this.state);

        public Element(params string[] names)
        {
            this.parent = null!;
            this.stage = null!;

            this.Selector = new Selector(this.GetType(), names);
        }

        protected internal virtual void Initialize(Stage stage, Element? parent)
        {
            this.stage = stage;
            this.parent = parent;

            _inline = this.stage.StyleSheet.Get<bool>(Property.Inline, this);
            _padding = this.stage.StyleSheet.Get<Padding>(Property.Padding, this);
            _width = this.stage.StyleSheet.Get<Unit>(Property.Width, this);
            _height = this.stage.StyleSheet.Get<Unit>(Property.Width, this);
            _alignment = this.stage.StyleSheet.Get<Alignment>(Property.Alignment, this);
        }

        protected internal virtual void Uninitialize()
        {
            this.parent = null;
            this.stage = null!;
        }

        protected internal virtual void Update(GameTime gameTime)
        {
        }

        protected internal virtual void Draw(GameTime gameTime, Vector2 position)
        {
            this.DrawOuter(gameTime, position += _outerBounds.Location.AsVector2());
            this.DrawInner(gameTime, position += _innerBounds.Location.AsVector2());

            this.stage.Screen.Graphics.PushScissorRectangle(new Rectangle()
            {
                X = (int)position.X,
                Y = (int)position.Y,
                Width = (int)_innerBounds.Width,
                Height = (int)_innerBounds.Height
            });

            this.DrawContent(gameTime, position += _contentAlignment);
            this.stage.Screen.Graphics.PopScissorRectangle();
        }

        protected virtual void DrawOuter(GameTime gameTime, Vector2 position)
        {
            _shape.Vertices[0].X = position.X;
            _shape.Vertices[0].Y = position.Y;

            _shape.Vertices[1].X = position.X + _outerBounds.Width;
            _shape.Vertices[1].Y = position.Y;

            _shape.Vertices[2].X = position.X + _outerBounds.Width;
            _shape.Vertices[2].Y = position.Y + _outerBounds.Height;

            _shape.Vertices[3].X = position.X;
            _shape.Vertices[3].Y = position.Y + _outerBounds.Height;

            _shape.Vertices[4].X = position.X;
            _shape.Vertices[4].Y = position.Y;

            this.stage.PrimitiveBatch.Trace(_shape, Color.Green, Matrix.Identity);
        }

        protected virtual void DrawInner(GameTime gameTime, Vector2 position)
        {
            _shape.Vertices[0].X = position.X;
            _shape.Vertices[0].Y = position.Y;

            _shape.Vertices[1].X = position.X + _innerBounds.Width;
            _shape.Vertices[1].Y = position.Y;

            _shape.Vertices[2].X = position.X + _innerBounds.Width;
            _shape.Vertices[2].Y = position.Y + _innerBounds.Height;

            _shape.Vertices[3].X = position.X;
            _shape.Vertices[3].Y = position.Y + _innerBounds.Height;

            _shape.Vertices[4].X = position.X;
            _shape.Vertices[4].Y = position.Y;

            this.stage.PrimitiveBatch.Trace(_shape, Color.Red, Matrix.Identity);
        }

        protected virtual void DrawContent(GameTime gameTime, Vector2 position)
        {
            _shape.Vertices[0].X = position.X - 1;
            _shape.Vertices[0].Y = position.Y - 1;

            _shape.Vertices[1].X = position.X + _contentBounds.Width + 1;
            _shape.Vertices[1].Y = position.Y - 1;

            _shape.Vertices[2].X = position.X + _contentBounds.Width + 1;
            _shape.Vertices[2].Y = position.Y + _contentBounds.Height + 1;

            _shape.Vertices[3].X = position.X - 1;
            _shape.Vertices[3].Y = position.Y + _contentBounds.Height + 1;

            _shape.Vertices[4].X = position.X - 1;
            _shape.Vertices[4].Y = position.Y - 1;

            this.stage.PrimitiveBatch.Trace(_shape, Color.Blue, Matrix.Identity);
        }

        protected internal virtual void Clean()
        {
            RectangleF outerConstraints = this.GetConstraints();
            RectangleF innerConstraints = outerConstraints;
            if (_padding.TryGetValue(out var padding))
            {
                padding.AddPadding(in outerConstraints, out innerConstraints);
            }

            _outerBounds = outerConstraints;
            _innerBounds = innerConstraints;

            this.CleanContentBounds(in innerConstraints, out _contentBounds);
            this.CleanInnerBounds(in innerConstraints, in _contentBounds, out _innerBounds);
            this.CleanOuterBounds(in outerConstraints, in _innerBounds, out _outerBounds);
            this.CleanContentAlignment(in _innerBounds, in _contentBounds, out _contentAlignment);
        }

        protected virtual RectangleF GetConstraints()
        {
            if (this.parent is null)
            {
                throw new NotImplementedException();
            }

            return this.parent.InnerBounds.Fit(_width, _height).SetLocation(PointF.Empty);
        }

        protected virtual void CleanOuterBounds(in RectangleF constraints, in RectangleF innerBounds, out RectangleF outerBounds)
        {
            outerBounds = constraints;

            if(this.Width is null)
            {
                outerBounds.Width = innerBounds.Width + (this.Padding?.Horizontal(constraints.Width) ?? 0);
            }

            if (this.Height is null)
            {
                outerBounds.Height = innerBounds.Height + (this.Padding?.Vertical(constraints.Height) ?? 0);
            }
        }

        protected virtual void CleanInnerBounds(in RectangleF constraints, in RectangleF contentBounds, out RectangleF innerBounds)
        {
            innerBounds = constraints;

            if (_width.Value is null)
            {
                innerBounds.Width = Math.Min(constraints.Width, contentBounds.Width);
            }

            if (_height.Value is null)
            {
                innerBounds.Height = Math.Min(constraints.Height, contentBounds.Height);
            }
        }

        protected virtual void CleanContentBounds(in RectangleF constraints, out RectangleF contentBounds)
        {
            contentBounds = new RectangleF();
        }

        protected virtual void CleanContentAlignment(in RectangleF innerBounds, in RectangleF contentBounds, out Vector2 contentAlignment)
        {
            PointF alignment = this.Alignment.Align(innerBounds.Size, contentBounds.Size);
            contentAlignment = alignment.AsVector2();
        }

        public virtual void SetPosition(Vector2 position)
        {
            _outerBounds.Location = position.AsPointFRef();
        }
        public void SetPosition(float? x = null, float? y = null)
        {
            _outerBounds.Location = new PointF()
            {
                X = x ?? _outerBounds.Location.X,
                Y = y ?? _outerBounds.Location.Y
            };
        }
    }
}
