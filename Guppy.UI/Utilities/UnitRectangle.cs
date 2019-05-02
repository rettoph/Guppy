using Guppy.Implementations;
using Guppy.Interfaces;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    public class UnitRectangle : TrackedDisposable
    {
        #region Private Fields
        public Rectangle _globalBounds;
        public Rectangle _relativeBounds;
        public Rectangle _localBounds;
        #endregion

        #region Public Attributes
        public Boolean Dirty { get { return this.DirtyBounds || this.DirtyPosition; } }
        public Boolean DirtyBounds { get; set; }
        public Boolean DirtyPosition { get; set; }

        public Unit X { get; }
        public Unit Y { get; }
        public Unit Width { get; }
        public Unit Height { get; }

        public UnitRectangle Parent { get; private set; }
        #endregion

        #region Public Fields
        /// <summary>
        /// The current rectangle's bounds relative to the global scope
        /// </summary>
        public Rectangle GlobalBounds { get { return _globalBounds; } }
        /// <summary>
        /// The current ractangle's bounds relative to its immediate parent
        /// </summary>
        public Rectangle RelativeBounds { get { return _relativeBounds; } }
        /// <summary>
        /// The current rectangle's bounds relative to itself
        /// </summary>
        public Rectangle LocalBounds { get { return _localBounds; } }
        #endregion

        #region Events
        public event EventHandler<Rectangle> OnBoundsChanged;
        public event EventHandler<Rectangle> OnPositionChanged;
        #endregion


        #region Constructors
        public UnitRectangle(Unit x, Unit y, Unit width, Unit height, UnitRectangle parent = null)
        {
            // Save the rectange bounds
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            this.setParent(parent);

            // Create a new output rectange to represent the calculated values
            _globalBounds = new Rectangle(0, 0, 0, 0);
            _relativeBounds = new Rectangle(0, 0, 0, 0);
            _localBounds = new Rectangle(0, 0, 0, 0);

            // Bind to events
            this.X.OnUpdated += this.HandlePositionValueUpdated;
            this.Y.OnUpdated += this.HandlePositionValueUpdated;
            this.Width.OnUpdated += this.HandleValueUpdated;
            this.Height.OnUpdated += this.HandleValueUpdated;

            // Clean at least once
            this.CleanBounds();
        }
        #endregion

        #region Methods
        public virtual void Update()
        {
            if (this.Dirty)
            {
                if (this.DirtyBounds)
                {
                    this.CleanBounds();
                    this.DirtyBounds = false;

                    this.OnBoundsChanged?.Invoke(this, this.GlobalBounds);
                }

                if (this.DirtyPosition)
                {
                    this.CleanPosition();
                    this.DirtyPosition = false;

                    this.OnPositionChanged?.Invoke(this, this.GlobalBounds);
                }
            }
        }

        public virtual void setParent(UnitRectangle parent)
        {
            if (parent != this.Parent)
            {
                if(this.Parent != null)
                    this.Parent.OnPositionChanged -= this.HandleParentPositionChanged;

                this.Parent = parent;

                this.X.SetParent(this.Parent.Width);
                this.Y.SetParent(this.Parent.Height);
                this.Width.SetParent(this.Parent.Width);
                this.Height.SetParent(this.Parent.Height);

                this.Parent.OnPositionChanged += this.HandleParentPositionChanged;

                this.DirtyBounds = true;
                this.DirtyPosition = true;
            }
        }
        #endregion

        #region Cleaning Methods
        protected virtual void CleanBounds()
        {
            // Update global bounds
            _globalBounds.Width = this.Width.Value;
            _globalBounds.Height = this.Height.Value;

            // Update relative bounds
            _relativeBounds.Width = this.Width.Value;
            _relativeBounds.Height = this.Height.Value;

            // Update local bounds
            _localBounds.Width = this.Width.Value;
            _localBounds.Height = this.Height.Value;
        }

        protected virtual void CleanPosition()
        {
            // Update global positions
            if (this.Parent == null)
            {
                _globalBounds.X = this.X.Value;
                _globalBounds.Y = this.Y.Value;
            }
            else
            {
                _globalBounds.X = this.Parent.GlobalBounds.X + this.X.Value;
                _globalBounds.Y = this.Parent.GlobalBounds.Y + this.Y.Value;
            }

            // Update relative positions
            _relativeBounds.X = this.X.Value;
            _relativeBounds.Y = this.Y.Value;
        }
        #endregion

        #region Event Handlers
        private void HandleValueUpdated(object sender, Unit e)
        {
            this.DirtyBounds = true;
        }

        private void HandlePositionValueUpdated(object sender, Unit e)
        {
            this.DirtyPosition = true;
        }

        private void HandleParentPositionChanged(object sender, Rectangle e)
        {
            this.DirtyPosition = true;
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            if (this.Parent != null)
                this.Parent.OnPositionChanged -= this.HandleParentPositionChanged;

            this.X.OnUpdated -= this.HandlePositionValueUpdated;
            this.Y.OnUpdated -= this.HandlePositionValueUpdated;
            this.Width.OnUpdated -= this.HandleValueUpdated;
            this.Height.OnUpdated -= this.HandleValueUpdated;

            this.X.Dispose();
            this.Y.Dispose();
            this.Width.Dispose();
            this.Height.Dispose();
        }

        #region Operators
        public static implicit operator Rectangle(UnitRectangle rect)
        {
            return rect.GlobalBounds;
        }
        #endregion
    }
}
