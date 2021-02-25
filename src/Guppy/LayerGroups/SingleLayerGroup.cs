using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.LayerGroups
{
    public sealed class SingleLayerGroup : LayerGroup
    {
        public readonly Int32 Value;

        public SingleLayerGroup(Int32 value)
        {
            this.Value = value;
        }

        public override Boolean Contains(Int32 group)
        {
            return this.Value == group;
        }

        public override Boolean Overlap(LayerGroup group)
        {
            return group.Contains(this.Value);
        }

        public override Int32 GetValue()
        {
            return this.Value;
        }
    }
}
