using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public class NetworkMessage : IData
    {
        public Byte RoomId { get; set; }
        public NetworkMessageConfiguration Configuration { get; set; }
        public IData Data { get; set; }
    }
}
