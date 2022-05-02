using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.Messages
{
    public struct ConnectionResponseMessage
    {
        public readonly int Id;
        public readonly Claim[] Claims;

        public ConnectionResponseMessage(int id, Claim[] claims)
        {
            this.Id = id;
            this.Claims = claims;
        }
    }
}
