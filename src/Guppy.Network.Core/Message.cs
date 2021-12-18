using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public class Message
    {
        public MessageConfiguration Configuration { get; set; }

        public IData Data { get; set; }
    }
}
