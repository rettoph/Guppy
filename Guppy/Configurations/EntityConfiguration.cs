using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Configurations
{
    public struct EntityConfiguration
    {
        public String Handle { get; internal set; }
        public Object Data { get; internal set; }
        public Type Type { get; internal set; }
    }
}
