using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using Serilog.Parsing;
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
        private static readonly IList<RowData> Rows = new List<RowData>();
        private struct RowData
        {
            public SizeF Size;
            public int Elements;

            public float AlignX(float maxRowWidth, HorizontalAlignment alignment)
            {
                return Alignment.AlignHorizontal(maxRowWidth, this.Size.Width, alignment);
            }
        }
        private readonly IList<T> _children;

        protected readonly ReadOnlyCollection<T> children;

        public Container(params string[] names) : base(names)
        {
            _children = new List<T>();

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
                child.TryDraw(gameTime, position);
            }
        }

        protected override void CleanContentBounds(in RectangleF constraints, out RectangleF contentBounds)
        {
            base.CleanContentBounds(constraints, out contentBounds);
            SizeF size = SizeF.Empty;
            
            Rows.Clear();
            RowData row = new RowData();
            float maxRowWidth = 0f;
            float rowsHeight = 0f;

            foreach (T child in _children)
            {
                child.Clean();

                if (child.Inline == false || row.Size.Width + child.OuterBounds.Width >= constraints.Width)
                {
                    Rows.Add(row);
                    maxRowWidth = MathF.Max(row.Size.Width, maxRowWidth);
                    rowsHeight += row.Size.Height;
                    row = new RowData();
                }

                row.Elements++;
                row.Size.Width += child.OuterBounds.Width;
                row.Size.Height = Math.Max(row.Size.Height, child.OuterBounds.Height);
            }

            if(row.Elements > 0)
            {
                Rows.Add(row);
                maxRowWidth = MathF.Max(row.Size.Width, maxRowWidth);
                rowsHeight += row.Size.Height;
            }

            if(Rows.Count == 0)
            {
                contentBounds.Width = 0;
                contentBounds.Height = 0;

                return;
            }

            int index = 0;
            int element = 0;
            row = Rows[index];
            PointF position = new PointF(row.AlignX(maxRowWidth, this.Alignment.Horizontal), 0);
            foreach (T child in _children)
            {
                if(element == row.Elements)
                {
                    position.Y += row.Size.Height;
                    row = Rows[++index];
                    position.X = row.AlignX(maxRowWidth, this.Alignment.Horizontal);
                    element = 0;
                }

                float y = Alignment.AlignVertical(row.Size.Height, child.OuterBounds.Height, this.Alignment.Vertical); ;
                child.SetPosition(position.X, position.Y + y);
                position.X += child.OuterBounds.Width;

                element++;
            }

            contentBounds.Width = maxRowWidth;
            contentBounds.Height = rowsHeight;
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
