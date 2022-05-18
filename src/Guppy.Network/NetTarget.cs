using Guppy.EntityComponent;
using Guppy.Network.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetTarget : Entity, INetTarget
    {
        public ushort NetId { get; private set; }
        ushort INetTarget.NetId
        {
            get => this.NetId;
            set => this.NetId = value;
        }

        public IMessageService Messages { get; private set; }
        IMessageService INetTarget.Messages
        {
            get => this.Messages;
            set => this.Messages = value;
        }
    }
}
