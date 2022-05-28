using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Messages
{
    public struct NetTargetActionMessage
    {
        public readonly ushort Id;
        public readonly uint TypeHash;
        public readonly NetTargetAction Action;

        public NetTargetActionMessage(ushort id, uint typeHash, NetTargetAction action)
        {
            this.Id = id;
            this.TypeHash = typeHash;
            this.Action = action;
        }
    }
}
