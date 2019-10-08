using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.LayerDepths
{
    internal sealed class SingleLayerDepth : LayerDepth
    {
        private Int32 _depth;

        internal SingleLayerDepth(Int32 depth)
        {
            _depth = depth;
        }

        public override bool Contains(Int32 depth)
        {
            return _depth == depth;
        }

        public override bool Overlap(LayerDepth depth)
        {
            return depth.Contains(_depth);
        }
    }
}
