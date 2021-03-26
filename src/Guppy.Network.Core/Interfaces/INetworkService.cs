using Guppy.Interfaces;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface INetworkService : IService
    {
        #region Properties
        /// <summary>
        /// The primary message manager, responsible for creating 
        /// messages to be sent and recieved by varius network services.
        /// </summary>
        MessageManager Messages { get; }
        #endregion
    }
}
