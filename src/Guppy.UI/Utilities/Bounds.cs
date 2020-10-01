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
                _x = value;
                this.OnChanged?.Invoke(_parent, this);
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
                _y = value;
                this.OnChanged?.Invoke(_parent, this);
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
                _width = value;
                this.OnChanged?.Invoke(_parent, this);
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
                _height = value;
                this.OnChanged?.Invoke(_parent, this);
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Invoked when the Width/Height or X/Y changes.
        /// </summary>
        public event OnBoundsChangedDelegate OnChanged;
        #endregion

        #region Constructor
        public Bounds(IElement parent)
        {
            _parent = parent;
        }
        #endregion
    }
}
