using Guppy.EntityComponent.Interfaces;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Interfaces
{
    public interface IMagicNetworkEntity : INetworkEntity
    {
        NetworkEntityMessageService Messages { get; }

        Pipe Pipe { get; set; }

        event OnChangedEventDelegate<IMagicNetworkEntity, Pipe> OnPipeChanged;
    }
}
