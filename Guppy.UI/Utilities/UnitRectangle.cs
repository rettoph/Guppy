using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    public class UnitRectangle
    {
        #region Private Fields
        public Rectangle _globalBounds;
        public Rectangle _relativeBounds;
        public Rectangle _localBounds;
        #endregion

        #region Public Attributes
        public Boolean DirtyBounds { get; set; }

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
        public event EventHandler<Rectangle> OnBoundsCleaned;
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
            this.X.OnUpdated += this.HandleValueUpdated;
            this.Y.OnUpdated += this.HandleValueUpdated;
            this.Width.OnUpdated += this.HandleValueUpdated;
            this.Height.OnUpdated += this.HandleValueUpdated;
        }
        #endregion

        #region Methods
        public virtual void Update()
        {
            if (this.DirtyBounds)
            {
                this.CleanBounds();

                this.DirtyBounds = false;
                this.OnBoundsCleaned?.Invoke(this, this.GlobalBounds);
            }
        }

        public virtual void setParent(UnitRectangle parent)
        {
            if (parent != this.Parent)
            {
                this.Parent = parent;

                this.X.SetParent(this.Parent.Width);
                this.Y.SetParent(this.Parent.Height);
                this.Width.SetParent(this.Parent.Width);
                this.Height.SetParent(this.Parent.Height);

                this.DirtyBounds = true;
            }
        }
        #endregion

        #region Cleaning Methods
        protected virtual void CleanBounds()
        {
            // Update global bounds
            if(this.Parent == null)
            {
                _globalBounds.X = this.X.Value;
                _globalBounds.Y = this.Y.Value;
            }
            else
            {
                _globalBounds.X = this.Parent.GlobalBounds.X + this.X.Value;
                _globalBounds.Y = this.Parent.GlobalBounds.Y + this.Y.Value;
            }
            
            _globalBounds.Width = this.Width.Value;
            _globalBounds.Height = this.Height.Value;

            // Update relative bounds
            _relativeBounds.X = this.X.Value;
            _relativeBounds.Y = this.Y.Value;
            _relativeBounds.Width = this.Width.Value;
            _relativeBounds.Height = this.Height.Value;

            // Update local bounds
            _localBounds.Width = this.Width.Value;
            _localBounds.Height = this.Height.Value;
        }
        #endregion

        #region Event Handlers
        private void HandleValueUpdated(object sender, Unit e)
        {
            this.DirtyBounds = true;
        }
        #endregion

        #region Operators
        public static implicit operator Rectangle(UnitRectangle rect)
        {
            return rect.GlobalBounds;
        }
        #endregion
    }
}
