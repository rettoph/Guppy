using Guppy.EntityComponent;
using Guppy.Network.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public interface INetTarget : IEntity
    {
        ushort NetId { get; internal set; }
        IMessageService Messages { get; internal set; }
    }
}
