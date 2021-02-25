using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.LayerGroups
{
    /// <summary>
    /// Simple layer group that can contain a range of values.
    /// </summary>
    public sealed class RangeLayerGroup : LayerGroup
    {
        public readonly Int32 Min;
        public readonly Int32 Max;

        public RangeLayerGroup(Int32 min, Int32 max)
        {
            this.Min = min;
            this.Max = max;
        }

        public override Boolean Contains(Int32 group)
        {
            return this.Min <= group && group <= this.Max;
        }

        public override Boolean Overlap(LayerGroup group)
        {
            return group.Contains(this.Min) || group.Contains(this.Max);
        }

        public override Int32 GetValue()
        {
            return this.Min;
        }
    }
}
