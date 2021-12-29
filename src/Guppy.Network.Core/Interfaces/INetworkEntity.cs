using Guppy.EntityComponent.Interfaces;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Interfaces
{
    public interface INetworkEntity : IEntity
    {
        UInt16 NetworkId { get; internal set; }
        Pipe Pipe { get; set; }
        NetworkEntityPacketService Messages { get; }

        event OnChangedEventDelegate<INetworkEntity, Pipe> OnPipeChanged;
    }
}
