using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.LayerDepths
{
    /// <summary>
    /// A layer index is used to determin what values
    /// a single layer resides on.
    /// </summary>
    public abstract class LayerDepth
    {
        /// <summary>
        /// Whether or not the current layer index
        /// contains the specified int value within.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract Boolean Contains(Int32 index);

        public abstract Boolean Overlap(LayerDepth depth);

        #region Implicit conversions
        public static implicit operator LayerDepth(Int32 depth)
        {
            return new SingleLayerDepth(depth);
        }

        public static implicit operator LayerDepth(Int32[] depths)
        {
            if (depths.Length != 2)
                throw new Exception($"Unknown layer depth range. Only 2 values alowed");
            else if (depths[1] <= depths[0])
                throw new Exception("Invalid layer depth range. Min value must come first.");

            return new RangeLayerDepths(depths[0], depths[1]);
        }

        public static implicit operator LayerDepth(LayerDepth[] depths)
        {
            return new MultiLayerIndexDepth(depths);
        }
        #endregion
    }
}
