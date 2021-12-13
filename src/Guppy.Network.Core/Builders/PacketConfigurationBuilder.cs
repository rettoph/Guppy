using DotNetUtils.General.Interfaces;
using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Guppy.Network.Builders
{
    public class PacketConfigurationBuilder : DataConfigurationBuilder<PacketConfigurationBuilder, PacketConfiguration>
    {
        #region Build Methods
        protected override PacketConfiguration Build(Byte[] idBytes)
        {
            return new PacketConfiguration(
                this.Id.Value,
                idBytes,
                this.Type,
                this.Writer,
                this.Reader);
        }
        #endregion
    }
}
