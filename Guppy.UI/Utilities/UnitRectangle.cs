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
        public Rectangle Bounds;
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
            this.Bounds = new Rectangle(this.X, this.Y, this.Width, this.Height);

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
                this.OnBoundsCleaned?.Invoke(this, this.Bounds);
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
            if(this.Parent == null)
            {
                this.Bounds.X = this.X.Value;
                this.Bounds.Y = this.Y.Value;
            }
            else
            {
                this.Bounds.X = this.Parent.Bounds.X + this.X.Value;
                this.Bounds.Y = this.Parent.Bounds.Y + this.Y.Value;
            }
            
            this.Bounds.Width = this.Width.Value;
            this.Bounds.Height = this.Height.Value;
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
            return rect.Bounds;
        }
        #endregion
    }
}
