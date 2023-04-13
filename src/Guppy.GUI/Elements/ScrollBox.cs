using Guppy.Common;
using Guppy.GUI.Constants;
using Guppy.Input.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class ScrollBox<T> : Container<T>, ISubscriber<CursorScroll>
        where T : Element
    {
        private IStyle<int> _trackWidth = null!;
        private IStyle<Color> _trackColor = null!;
        private IStyle<Color> _thumbColor = null!;
        private RectangleF _trackBounds;
        private RectangleF _thumbBounds;

        public int? TrackWidth => _trackWidth.GetValue(this.State);
        public Color? TrackCOlor => _trackColor.GetValue(this.State);
        public Color? ThumbColor => _thumbColor.GetValue(this.State);

        public RectangleF TrackBounds => _trackBounds;
        public RectangleF ThumbBounds => _thumbBounds;

        public ScrollBox(params string[] names) : base(names.Concat(new[] { ElementNames.ScrollBox  }).ToArray())
        {
        }

        protected internal override void Initialize(Stage stage, Element? parent)
        {
            base.Initialize(stage, parent);

            _trackWidth = this.stage.StyleSheet.Get(Property.ScrollTrackWidth, this);
            _trackColor = this.stage.StyleSheet.Get(Property.ScrollTrackColor, this);
            _thumbColor = this.stage.StyleSheet.Get(Property.ScrollThumbColor, this);

            this.stage.Bus.Subscribe(this);
        }

        protected internal override void Uninitialize()
        {
            base.Uninitialize();

            this.stage.Bus.Unsubscribe(this);
        }

        protected override void DrawOuter(GameTime gameTime, Vector2 position)
        {
            base.DrawOuter(gameTime, position);

            if(_trackColor.TryGetValue(this.State, out var trackColor))
            {
                this.stage.SpriteBatch.Draw(
                    texture: this.stage.Pixel,
                    destinationRectangle: new Rectangle(
                        x: (int)(position.X + _trackBounds.X), 
                        y: (int)(position.Y + _trackBounds.Y), 
                        width: (int)_trackBounds.Width, 
                        height: (int)_trackBounds.Height),
                    color: trackColor);
            }

            if (_thumbColor.TryGetValue(this.State, out var thumbColor))
            {
                this.stage.SpriteBatch.Draw(
                    texture: this.stage.Pixel,
                    destinationRectangle: new Rectangle(
                        x: (int)(position.X + _thumbBounds.X),
                        y: (int)(position.Y + _thumbBounds.Y),
                        width: (int)_thumbBounds.Width,
                        height: (int)_thumbBounds.Height),
                    color: thumbColor);
            }
        }

        protected override RectangleF GetInnerConstraints(in RectangleF outerConstraints)
        {
            RectangleF innerConstraints = base.GetInnerConstraints(outerConstraints);

            if (_trackWidth.TryGetValue(out var width))
            {
                innerConstraints.Width -= width;
            }

            return innerConstraints;
        }

        protected override void CleanOuterBounds(in RectangleF constraints, in RectangleF innerBounds, out RectangleF outerBounds)
        {
            base.CleanOuterBounds(constraints, innerBounds, out outerBounds);

            if (_trackWidth.TryGetValue(out var width))
            {
                if (this.Width is null)
                {
                    outerBounds.Width += _trackBounds.Width;
                }

                _trackBounds.Width = width;
                _trackBounds.X = outerBounds.Width - _trackBounds.Width;
                _trackBounds.Height = outerBounds.Height;
                _trackBounds.Y = outerBounds.Y;

                _thumbBounds.Width = width;
                _thumbBounds.X = _trackBounds.X;
                _thumbBounds.Height = _trackBounds.Height * (this.InnerBounds.Height / this.ContentBounds.Height);
                _thumbBounds.Y = outerBounds.Y;
            }
        }

        protected override void CleanInnerBounds(in RectangleF constraints, in RectangleF contentBounds, out RectangleF innerBounds)
        {
            base.CleanInnerBounds(constraints, contentBounds, out innerBounds);
        }

        protected override void CleanContentOffset(in RectangleF innerBounds, in RectangleF contentBounds, out Vector2 contentOffset)
        {
            base.CleanContentOffset(innerBounds, contentBounds, out contentOffset);

            this.ScrollTo(this.contentOffset.Y);
        }

        private void ScrollTo(float scroll)
        {
            float minimum = this.InnerBounds.Height - this.ContentBounds.Height;

            if(minimum > 0)
            {
                return;
            }

            scroll = Math.Clamp(scroll, minimum, 0);
            float ratio = scroll / minimum;
            float thumbScroll = ratio * (_trackBounds.Height + 1 - _thumbBounds.Height);

            this.contentOffset.Y = scroll;
            _thumbBounds.Y = thumbScroll;
        }

        public void Process(in CursorScroll message)
        {
            if(!this.State.HasFlag(ElementState.Hovered))
            {
                return;
            }

            this.ScrollTo(this.contentOffset.Y + message.Delta);
        }
    }
}
