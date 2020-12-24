using Guppy.UI.Delegates;
using Guppy.UI.Interfaces;
using Guppy.UI.Utilities.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    public class Bounds
    {
        #region Private Fields
        private Unit _x = 0;
        private Unit _y = 0;
        private Unit _width = 1f;
        private Unit _height = 1f;
        private IElement _parent;
        #endregion

        #region Public Properties
        /// <summary>
        /// The X, generally compared to
        /// the OuterBounds size.
        /// </summary>
        public Unit X
        {
            get => _x;
            set
            {
                if (!_x.Equals(value))
                {
                    _x = value;
                    this.OnChanged?.Invoke(_parent, this);
                    this.OnPositionChanged?.Invoke(_parent, this);
                }
            }
        }

        /// <summary>
        /// The Y, generally compared to
        /// the OuterBounds size.
        /// </summary>
        public Unit Y
        {
            get => _y;
            set
            {
                if (!_y.Equals(value))
                {
                    _y = value;
                    this.OnChanged?.Invoke(_parent, this);
                    this.OnPositionChanged?.Invoke(_parent, this);
                }
            }
        }

        /// <summary>
        /// The width, generally compared to
        /// the OuterBounds size.
        /// </summary>
        public Unit Width
        {
            get => _width;
            set
            {
                if (!_width.Equals(value))
                {
                    _width = value;
                    this.OnChanged?.Invoke(_parent, this);
                    this.OnSizeChanged?.Invoke(_parent, this);
                }
            }
        }

        /// <summary>
        /// The height, generally compared to
        /// the OuterBounds size.
        /// </summary>
        public Unit Height
        {
            get => _height;
            set
            {
                if (!_height.Equals(value))
                {
                    _height = value;
                    this.OnChanged?.Invoke(_parent, this);
                    this.OnSizeChanged?.Invoke(_parent, this);
                }
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Invoked when the Width/Height or X/Y changes.
        /// </summary>
        public event OnBoundsChangedDelegate OnChanged;

        /// <summary>
        /// Invoked when the X/Y changes.
        /// </summary>
        public event OnBoundsChangedDelegate OnPositionChanged;

        /// <summary>
        /// Invoked when the Width/Height changes.
        /// </summary>
        public event OnBoundsChangedDelegate OnSizeChanged;
        #endregion

        #region Constructor
        public Bounds(IElement parent)
        {
            _parent = parent;
        }
        #endregion
    }
}
