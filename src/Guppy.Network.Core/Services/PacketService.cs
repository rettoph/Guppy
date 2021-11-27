using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Services
{
    public sealed class PacketService : DataService<PacketConfiguration>
    {
        internal PacketService(
            DynamicIdSize idSize,
            PacketConfiguration[] configurations) : base(idSize, configurations)
        {
        }
    }
}
