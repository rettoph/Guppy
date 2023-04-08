using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class Container<T> : Element
        where T : Element
    {
        private readonly IList<T> _children;
        private readonly IList<T> _row;

        protected readonly ReadOnlyCollection<T> children;

        public Container(params string[] names) : base(names)
        {
            _children = new List<T>();
            _row = new List<T>();

            this.children = new ReadOnlyCollection<T>(_children);
        }

        protected virtual void Add(T child)
        {
            _children.Add(child);

            child.Initialize(this.stage, this);
            this.Clean();
        }

        protected virtual void Remove(T child)
        {
            if(!_children.Remove(child))
            {
                return;
            }

            child.Uninitialize();
            this.Clean();
        }

        protected override void DrawContent(GameTime gameTime, Vector2 position)
        {
            base.DrawContent(gameTime, position);

            foreach (T child in _children)
            {
                child.Draw(gameTime, position);
            }
        }

        protected internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (T child in _children)
            {
                child.Update(gameTime);
            }
        }

        protected override void CleanContentBounds(in RectangleF constraints, out RectangleF contentBounds)
        {
            base.CleanContentBounds(constraints, out contentBounds);
            Vector2 size = Vector2.Zero;
            Vector2 rowSize = Vector2.Zero;
            float rowY = 0f;
            _row.Clear();

            foreach (T child in _children)
            {
                child.Clean();

                if (child.Inline == false || rowSize.X + child.OuterBounds.Width >= constraints.Width)
                {
                    size.Y += rowSize.Y;
                    size.X = Math.Max(size.X, rowSize.X);

                    this.VerticalAlignElements(_row, rowY, rowSize.Y);

                    rowY += rowSize.Y;
                    rowSize = Vector2.Zero;
                    _row.Clear();
                }

                child.SetPosition(rowSize.X, null);
                _row.Add(child);
                rowSize.X += child.OuterBounds.Width;
                rowSize.Y = Math.Max(rowSize.Y, child.OuterBounds.Height);
            }

            if(_row.Count > 0)
            {
                size.Y += rowSize.Y;
                size.X = Math.Max(size.X, rowSize.X);

                this.VerticalAlignElements(_row, rowY, rowSize.Y);
                _row.Clear();
            }

            contentBounds.Width = size.X;
            contentBounds.Height = size.Y;
        }

        private void VerticalAlignElements(IEnumerable<T> elements, float y, float height)
        {
            foreach (T element in elements)
            {
                element.SetPosition(null, y + (height - element.OuterBounds.Height) / 2);
            }
        }
    }
}
