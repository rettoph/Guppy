using System;

namespace Guppy.Configurations
{
    public class LayerConfiguration
    {
        public readonly UInt16 MinDepth;
        public readonly UInt16 MaxDepth;

        public readonly UInt16 UpdateOrder;
        public readonly UInt16 DrawOrder;

        public LayerConfiguration(UInt16 minDepth = 0, UInt16 maxDepth = 0, UInt16 updateOrder = 0, UInt16 drawOrder = 0)
        {
            if (maxDepth < minDepth)
                throw new Exception($"Invalid LayerConfiguration Parameters. Max({maxDepth}) value less that Min({minDepth}) value");

            this.MinDepth = minDepth;
            this.MaxDepth = maxDepth;

            this.UpdateOrder = updateOrder;
            this.DrawOrder = drawOrder;
        }
    }
}
