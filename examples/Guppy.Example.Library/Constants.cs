using Guppy.Contexts;
using Guppy.LayerGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library
{
    public static class Constants
    {
        public const Int32 WorldWidth = 800;
        public const Int32 WorldHeight = 480;

        public static class LayerContexts
        {
            public static readonly LayerContext Foreground = new LayerContext()
            {
                DrawOrder = 0,
                Visible = true,
                UpdateOrder = 0,
                Enabled = true,
                Group = new SingleLayerGroup(0)
            };
        }

        public static class Intervals
        {
            public const UInt16 PositionMessage = 250;
            public const UInt16 TargetMessage = 60;
        }
    }
}
