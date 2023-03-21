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

        private IStyle<bool> _inline;
        private IStyle<Unit> _width;
        private IStyle<Unit> _height;
        private IStyle<Padding> _padding;

        protected Stage stage;
        protected Element? parent;
        protected ElementState state;
        protected Point position;
        protected Rectangle bounds;
        protected Rectangle content;

        public Selector Selector { get; }
        public ElementState State => this.state;
        public Point Position => this.position;

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
        }

        protected internal virtual void Uninitialize()
        {
            this.parent = null;
            this.stage = null!;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, Point position)
        {
            this.OuterDraw(gameTime, position + this.bounds.Location);
        }

        protected virtual void OuterDraw(GameTime gameTime, Point position)
        {
            _shape.Vertices[0].X = position.X;
            _shape.Vertices[0].Y = position.Y;

            _shape.Vertices[1].X = position.X + this.bounds.Width;
            _shape.Vertices[1].Y = position.Y;

            _shape.Vertices[2].X = position.X + this.bounds.Width;
            _shape.Vertices[2].Y = position.Y + this.bounds.Height;

            _shape.Vertices[3].X = position.X;
            _shape.Vertices[3].Y = position.Y + this.bounds.Height;

            _shape.Vertices[4].X = position.X;
            _shape.Vertices[4].Y = position.Y;

            this.stage.PrimitiveBatch.Trace(_shape, Color.Red, Matrix.Identity);

            this.InnerDraw(gameTime, position + this.content.Location);
        }

        protected virtual void InnerDraw(GameTime gameTime, Point position)
        {
            _shape.Vertices[0].X = position.X;
            _shape.Vertices[0].Y = position.Y;

            _shape.Vertices[1].X = position.X + this.content.Width;
            _shape.Vertices[1].Y = position.Y;

            _shape.Vertices[2].X = position.X + this.content.Width;
            _shape.Vertices[2].Y = position.Y + this.content.Height;

            _shape.Vertices[3].X = position.X;
            _shape.Vertices[3].Y = position.Y + this.content.Height;

            _shape.Vertices[4].X = position.X;
            _shape.Vertices[4].Y = position.Y;

            this.stage.PrimitiveBatch.Trace(_shape, Color.Blue, Matrix.Identity);
        }

        protected internal virtual void Clean(out Point size)
        {
            Point constraints = this.GetSizeConstraints();

            // Reset positions
            this.bounds.Location = Point.Zero;
            this.content.Location = Point.Zero;

            // Set the content constraints
            this.content.Width = constraints.X;
            this.content.Height = constraints.Y;
            _padding.Value?.AddPadding(ref this.content);

            // Snap to fit to the calculated size
            this.content.Size = this.CleanContentSize(this.content.Size);
            this.bounds.Size = this.CleanBoundsSize(constraints, this.content.Size);

            size = this.bounds.Size;
        }

        protected virtual Point GetSizeConstraints()
        {
            if(this.parent is null)
            {
                throw new NotImplementedException();
            }

            return this.parent.content.Size;
        }

        protected virtual Point CleanContentSize(Point constraints)
        {
            return Point.Zero;
        }

        protected virtual Point CleanBoundsSize(Point constraints, Point content)
        {
            Point result;

            if(_width.TryGetValue(out var width))
            {
                result.X = width.Calculate(constraints.X);
            }
            else
            {
                result.X = Math.Min(constraints.X, content.X);
            }

            if (_height.TryGetValue(out var height))
            {
                result.Y = height.Calculate(constraints.Y);
            }
            else
            {
                result.Y = Math.Min(constraints.Y, content.Y);
            }

            return result;
        }
    }
}
