using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;

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
        private readonly IList<T> _visible;

        protected readonly ReadOnlyCollection<T> children;
        protected readonly ReadOnlyCollection<T> visible;

        public Container(params string[] names) : this((IEnumerable<string>)names)
        {
        }
        public Container(IEnumerable<string> names) : base(names)
        {
            _children = new List<T>();
            _visible = new List<T>();

            this.children = new ReadOnlyCollection<T>(_children);
            this.visible = new ReadOnlyCollection<T>(_visible);
        }

        protected internal override void Initialize(Stage stage, Element? parent)
        {
            foreach (T child in _children)
            {
                child.Initialize(stage, this);
            }

            base.Initialize(stage, parent);
        }

        protected internal override void Uninitialize()
        {
            base.Uninitialize();

            foreach (T child in _children)
            {
                child.Uninitialize();
            }
        }

        public virtual void Add(T child)
        {
            _children.Add(child);
            child.OnVisibleChanged += this.HandleVisibleChanged;

            if(!this.Initialized)
            {
                return;
            }

            child.Initialize(this.stage, this);
            (this.parent ?? this)?.Clean();
        }

        public virtual void Remove(T child)
        {
            if(!_children.Remove(child))
            {
                return;
            }

            child.OnVisibleChanged -= this.HandleVisibleChanged;

            if (!this.Initialized)
            {
                return;
            }

            child.Uninitialize();
            this.Clean();
        }

        protected override void DrawContent(GameTime gameTime, Vector2 position)
        {
            base.DrawContent(gameTime, position);

            foreach (T child in _visible)
            {
                child.TryDraw(gameTime, position);
            }
        }

        protected internal override void Clean()
        {
            _visible.Clear();
            foreach (T child in _children)
            {
                if (child.Visible)
                {
                    _visible.Add(child);
                }
            }

            base.Clean();
        }

        protected override void CleanContentBounds(in RectangleF constraints, out RectangleF contentBounds)
        {
            base.CleanContentBounds(constraints, out contentBounds);
            SizeF size = SizeF.Empty;
            
            Rows.Clear();
            RowData row = new RowData();
            float maxRowWidth = 0f;
            float rowsHeight = 0f;

            foreach (T child in _visible)
            {
                child.Clean();

                if (row.Elements > 0 && (child.Inline == false || row.Size.Width + child.OuterBounds.Width >= constraints.Width))
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
            foreach (T child in _visible)
            {
                if (element == row.Elements)
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

        private void HandleVisibleChanged(Element sender, bool old, bool value)
        {
            this.Clean();
        }
    }
}
