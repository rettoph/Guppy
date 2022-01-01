using Guppy.EntityComponent.Interfaces;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Interfaces
{
    /// <summary>
    /// Defines a simple entity that contains a network id.
    /// This value will automatically get defined within 
    /// remote master peers.
    /// </summary>
    public interface INetworkEntity : IEntity
    {
        NetworkEntityMessageService Messages { get; }
        UInt16 NetworkId { get; internal set; }
    }
}
