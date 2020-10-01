using Guppy.UI.Delegates;
using Guppy.UI.Interfaces;
using Guppy.UI.Utilities.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    public class Padding
    {
        #region Private Fields
        private Unit _top = 0;
        private Unit _right = 0;
        private Unit _bottom = 0;
        private Unit _left = 0;
        IElement _parent;
        #endregion

        #region Public Properties
        /// <summary>
        /// The top padding, generally compared to
        /// the OuterBounds size.
        /// </summary>
        public Unit Top
        {
            get => _top;
            set
            {
                _top = value;
                this.OnChanged?.Invoke(_parent, this);
            }
        }

        /// <summary>
        /// The right padding, generally compared to
        /// the OuterBounds size.
        /// </summary>
        public Unit Right
        {
            get => _right;
            set
            {
                _right = value;
                this.OnChanged?.Invoke(_parent, this);
            }
        }

        /// <summary>
        /// The bottom padding, generally compared to
        /// the OuterBounds size.
        /// </summary>
        public Unit Bottom
        {
            get => _bottom;
            set
            {
                _bottom = value;
                this.OnChanged?.Invoke(_parent, this);
            }
        }

        /// <summary>
        /// The left padding, generally compared to
        /// the OuterBounds size.
        /// </summary>
        public Unit Left
        {
            get => _left;
            set
            {
                _left = value;
                this.OnChanged?.Invoke(_parent, this);
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Invoked when any of the internal padding values change.
        /// </summary>
        public event OnPaddingChangedDelegate OnChanged;
        #endregion

        #region Constructor
        public Padding(IElement parent)
        {
            _parent = parent;
        }
        #endregion
    }
}
