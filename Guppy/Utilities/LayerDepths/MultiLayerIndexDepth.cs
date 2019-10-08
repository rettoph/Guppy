using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.LayerDepths
{
    internal sealed class MultiLayerIndexDepth : LayerDepth
    {
        private LayerDepth[] _depths;

        internal MultiLayerIndexDepth(LayerDepth[] depths)
        {
            _depths = depths;
        }

        public override bool Contains(Int32 index)
        {
            foreach (LayerDepth li in _depths)
                if (li.Contains(index))
                    return true;

            return false;
        }

        public override bool Overlap(LayerDepth depth)
        {
            foreach (LayerDepth li in _depths)
                if (li.Overlap(li))
                    return true;

            return false;
        }
    }
}
