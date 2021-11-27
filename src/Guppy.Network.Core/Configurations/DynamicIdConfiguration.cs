using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    public abstract class DynamicIdConfiguration
    {
        public readonly UInt16 Id;

        public readonly Byte[] IdBytes;

        public DynamicIdConfiguration(
            UInt16 id,
            Byte[] idBytes)
        {
            this.Id = id;
            this.IdBytes = idBytes;
        }
    }
}
