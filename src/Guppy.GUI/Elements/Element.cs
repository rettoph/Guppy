using Guppy.MonoGame.Extensions.Primitives;
using Guppy.MonoGame.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class Element
    {
        private static PrimitiveShape _shape = new PrimitiveShape(new Vector3[5]);

        private Rectangle _outerBounds;
        private Rectangle _innerBounds;
        private Rectangle _contentBounds;
        private Point _contentAlignment;
        private IStyle<Unit> _width = null!;
        private IStyle<Unit> _height = null!;
        private IStyle<Padding> _padding = null!;

        protected Stage stage;
        protected Element? parent;
        protected ElementState state;

        public Selector Selector { get; }
        public ElementState State => this.state;

        public Rectangle OuterBounds => _outerBounds;
        public Rectangle InnerBounds => _innerBounds;
        public Rectangle ContentBounds => _contentBounds;

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

            _padding = this.stage.StyleSheet.Get<Padding>(Property.Padding, this);
            _width = this.stage.StyleSheet.Get<Unit>(Property.Width, this);
            _height = this.stage.StyleSheet.Get<Unit>(Property.Width, this);
        }

        protected internal virtual void Uninitialize()
        {
            this.parent = null;
            this.stage = null!;
        }

        protected internal virtual void Update(GameTime gameTime)
        {
        }

        protected internal virtual void Draw(GameTime gameTime, Point position)
        {
            this.DrawOuter(gameTime, position);
            this.DrawInner(gameTime, position + _innerBounds.Location);
            this.DrawContent(gameTime, position + _contentAlignment);
        }

        protected virtual void DrawOuter(GameTime gameTime, Point position)
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

        protected virtual void DrawInner(GameTime gameTime, Point position)
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

        protected virtual void DrawContent(GameTime gameTime, Point position)
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
            Rectangle outerConstraints = this.GetConstraints();
            Rectangle innerConstraints = outerConstraints;
            if (_padding.TryGetValue(out var padding))
            {
                padding.AddPadding(in outerConstraints, out innerConstraints);
            }


            this.CleanContentBounds(in innerConstraints, out _contentBounds);
            this.CleanInnerBounds(in innerConstraints, in _contentBounds, out _innerBounds);
            this.CleanOuterBounds(in outerConstraints, out _outerBounds);
            this.CleanContentAlignment(in _innerBounds, in _contentBounds, out _contentAlignment);
        }

        protected virtual Rectangle GetConstraints()
        {
            if (this.parent is null)
            {
                throw new NotImplementedException();
            }

            return this.parent.InnerBounds.Fit(_width, _height);
        }

        protected virtual void CleanOuterBounds(in Rectangle constraints, out Rectangle outerBounds)
        {
            outerBounds = constraints;
        }

        protected virtual void CleanInnerBounds(in Rectangle constraints, in Rectangle contentBounds, out Rectangle innerBounds)
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

        protected virtual void CleanContentBounds(in Rectangle constraints, out Rectangle contentBounds)
        {
            contentBounds = new Rectangle();
        }

        protected virtual void CleanContentAlignment(in Rectangle innerBounds, in Rectangle contentBounds, out Point contentAlignment)
        {
            int x = innerBounds.Width - contentBounds.Width;
            x /= 2;
            x += innerBounds.Location.X;

            int y = innerBounds.Height - contentBounds.Height;
            y /= 2;
            y += innerBounds.Location.Y;

            contentAlignment = new Point(x, y);
        }
    }
}
