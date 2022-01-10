using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Interfaces;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public class NetworkEntity : Entity, INetworkEntity
    {
        #region Public Properties
        public UInt16 NetworkId { get; set; }
        #endregion
    }
}
