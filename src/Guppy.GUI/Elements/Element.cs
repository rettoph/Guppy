using Guppy.GUI.Extensions.Microsoft.Xna.Framework;
using Guppy.GUI.Extensions.System.Drawing;
using Guppy.MonoGame.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Guppy.GUI.Elements
{
    public class Element
    {
        private static int CurrentId = 0;

        private ElementState _state;
        private RectangleF _outerBounds;
        private RectangleF _innerBounds;
        private RectangleF _contentBounds;
        private IStyle<bool> _inline = null!;
        private IStyle<Unit> _width = null!;
        private IStyle<Unit> _height = null!;
        private IStyle<Padding> _padding = null!;
        private IStyle<Alignment> _alignment = null!;
        private IStyle<Color> _backgroundColor = null!;
        private IStyle<Color> _borderColor = null!;
        private IStyle<Unit> _borderThickness = null!;

        protected Stage stage;
        protected Element? parent;
        protected Vector2 contentOffset;

        public readonly int Id;
        public Selector Selector { get; private set; }
        public bool Initialized { get; protected set; }
        public ElementState State
        {
            get => _state;
            set => this.OnStateChanged.InvokeIf(_state != value, this, ref _state, value);
        }

        public RectangleF OuterBounds => _outerBounds;
        public RectangleF InnerBounds => _innerBounds;
        public RectangleF ContentBounds => _contentBounds;
        public Vector2 ContentOffset => this.contentOffset;

        public bool Inline => _inline.GetValue(this.State);
        public Unit? Width => _width.GetValue(this.State);
        public Unit? Height => _height.GetValue(this.State);
        public Padding? Padding => _padding.GetValue(this.State);
        public Alignment Alignment => _alignment.GetValue(this.State);
        public Color BackgroundColor => _backgroundColor.GetValue(this.State);
        public Color BorderColor => _borderColor.GetValue(this.State);
        public Unit? BorderThickness => _borderThickness.GetValue(this.State);

        public event OnChangedEventDelegate<Element, ElementState>? OnStateChanged;

        public Element(params string[] names)
        {
            this.Id = CurrentId++;
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
            _height = this.stage.StyleSheet.Get<Unit>(Property.Height, this);
            _alignment = this.stage.StyleSheet.Get<Alignment>(Property.Alignment, this);
            _backgroundColor = this.stage.StyleSheet.Get<Color>(Property.BackgroundColor, this);
            _borderColor = this.stage.StyleSheet.Get<Color>(Property.BorderColor, this);
            _borderThickness = this.stage.StyleSheet.Get<Unit>(Property.BorderThickness, this);

            this.Selector.Parent = parent?.Selector;
            this.Initialized = true;
        }

        protected internal virtual void Uninitialize()
        {
            this.parent = null;
            this.stage = null!;

            this.Initialized = false;
        }

        protected virtual void CleanState(GameTime gameTime, Vector2 position)
        {
            bool wasHovered = this.State.HasFlag(ElementState.Hovered);
            bool isHovered = position.X <= this.stage.Mouse.Position.X;
            isHovered &= this.stage.Mouse.Position.X <= position.X + this.OuterBounds.Width;
            isHovered &= position.Y <= this.stage.Mouse.Position.Y;
            isHovered &= this.stage.Mouse.Position.Y <= position.Y + this.OuterBounds.Height; 

            if(isHovered && !wasHovered)
            {
                this.State |= ElementState.Hovered;
            }
            else if(!isHovered && wasHovered)
            {
                this.State &= ~ElementState.Hovered;
            }
        }

        protected internal virtual bool TryDraw(GameTime gameTime, Vector2 position)
        {
            position += _outerBounds.Location.AsVector2();

            BoundingBox boundingBox = new BoundingBox(
                min: new Vector3(position.X, position.Y, 0),
                max: new Vector3(position.X + this.OuterBounds.Width, position.Y + this.OuterBounds.Height, 0));

            ContainmentType containmentType = this.stage.Screen.Camera.Frustum.Contains(boundingBox);
            if (containmentType == ContainmentType.Disjoint)
            {
                return false;
            }

            this.Draw(gameTime, position);
            return true;
        }
        protected virtual void Draw(GameTime gameTime, Vector2 position)
        {
            this.CleanState(gameTime, position);
            this.DrawOuter(gameTime, position);

            this.DrawInner(gameTime, position += _innerBounds.Location.AsVector2());

            // this.stage.Screen.Graphics.PushScissorRectangle(new Rectangle()
            // {
            //     X = (int)position.X,
            //     Y = (int)position.Y,
            //     Width = (int)_innerBounds.Width,
            //     Height = (int)_innerBounds.Height
            // });

            this.DrawContent(gameTime, position += this.contentOffset);
            // this.stage.Screen.Graphics.PopScissorRectangle();
        }

        protected virtual void DrawOuter(GameTime gameTime, Vector2 position)
        {
            if(_backgroundColor.TryGetValue(this.State, out var color))
            {
                this.stage.SpriteBatch.Draw(
                    texture: this.stage.Pixel,
                    destinationRectangle: new Rectangle((int)position.X, (int)position.Y, (int)this.OuterBounds.Width, (int)this.OuterBounds.Height),
                    color: color);
            }

            if(_borderColor.TryGetValue(this.State, out var borderColor) &&
                _borderThickness.TryGetValue(this.State, out var borderThickness))
            {
                float thicknessX = borderThickness.Calculate(this.OuterBounds.Width);
                float thicknessY = borderThickness.Calculate(this.OuterBounds.Height);

                var topLeft = new Vector2(0, 0);
                var bottomLeft = new Vector2(0, this.OuterBounds.Height);
                var topRight = new Vector2(this.OuterBounds.Width, 0);
                var bottomRight = new Vector2(this.OuterBounds.Width, this.OuterBounds.Height);
                
                this.stage.SpriteBatch.DrawLine(position, position + topRight, borderColor, thicknessX);
                this.stage.SpriteBatch.DrawLine(position + topRight, position + bottomRight, borderColor, thicknessX);
                this.stage.SpriteBatch.DrawLine(position + bottomLeft, position + bottomRight, borderColor, thicknessX);
                this.stage.SpriteBatch.DrawLine(position + bottomLeft, position + topLeft, borderColor, thicknessX);
            }
        }

        protected virtual void DrawInner(GameTime gameTime, Vector2 position)
        {
        }

        protected virtual void DrawContent(GameTime gameTime, Vector2 position)
        {
        }

        protected internal virtual void Clean()
        {
            RectangleF outerConstraints = this.GetOuterConstraints();
            RectangleF innerConstraints = this.GetInnerConstraints(in outerConstraints);
            

            _outerBounds = outerConstraints;
            _innerBounds = innerConstraints;

            this.CleanContentBounds(in innerConstraints, out _contentBounds);
            this.CleanInnerBounds(in innerConstraints, in _contentBounds, out _innerBounds);
            this.CleanOuterBounds(in outerConstraints, in _innerBounds, out _outerBounds);
            this.CleanContentOffset(in _innerBounds, in _contentBounds, out this.contentOffset);
        }

        protected virtual RectangleF GetOuterConstraints()
        {
            if (this.parent is null)
            {
                throw new NotImplementedException();
            }

            return this.parent.InnerBounds.Fit(_width, _height).SetLocation(this.OuterBounds.Location);
        }

        protected virtual RectangleF GetInnerConstraints(in RectangleF outerConstraints)
        {
            RectangleF innerConstraints = outerConstraints;

            if (_padding.TryGetValue(out var padding))
            {
                padding.AddPadding(in outerConstraints, out innerConstraints);
            }

            return innerConstraints;
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

        protected virtual void CleanContentOffset(in RectangleF innerBounds, in RectangleF contentBounds, out Vector2 contentOffset)
        {
            PointF alignment = this.Alignment.Align(innerBounds.Size, contentBounds.Size);
            contentOffset = alignment.AsVector2();
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
