using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    /// <summary>
    /// A simple unit that contains multiple internal units
    /// and will sum them together.
    /// </summary>
    public class MultiUnit : Unit
    {
        #region Private Fields
        private Unit[] _children;
        #endregion

        #region Constructor
        public MultiUnit(params Unit[] children)
        {
            _children = children;
        }
        #endregion

        #region Unit Implementation
        public override Unit Flip()
            => new MultiUnit(_children.Select(c => c.Flip()).ToArray());

        public override int ToPixel(int parent)
            => _children.Sum(c => c.ToPixel(parent));
        #endregion

        public override bool Equals(object obj)
        {
            if (obj is MultiUnit b)
                if (Enumerable.SequenceEqual(_children, b._children))
                    return true;

            return false;
        }
    }
}
