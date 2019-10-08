using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.LayerDepths
{
    internal sealed class RangeLayerDepths : LayerDepth
    {
        private Int32 _min;
        private Int32 _max;

        internal RangeLayerDepths(Int32 min, Int32 max)
        {
            _min = min;
            _max = max;
        }

        public override bool Contains(Int32 depth)
        {
            return _min <= depth && depth <= _max;
        }

        public override bool Overlap(LayerDepth depth)
        {
            return depth.Contains(_min) || depth.Contains(_max);
        }
    }
}
