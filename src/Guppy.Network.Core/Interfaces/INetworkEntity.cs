using Guppy.Interfaces;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface INetworkEntity : IEntity
    {
        #region Properties
        IPipe Pipe { get; set; }
        MessageManager Messages { get; }
        #endregion

        #region Events
        event OnChangedEventDelegate<INetworkEntity, IPipe> OnPipeChanged;
        #endregion
    }
}
