using Guppy.Interfaces;
using Guppy.Network.Contexts;
using Guppy.Network.Lists;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface IChannel : IEntity
    {
        #region Properties
        /// <summary>
        /// A short id used internally to differentiate
        /// specific channels.
        /// </summary>
        new Int16 Id { get; internal set; }

        /// <summary>
        /// The current user pipe.
        /// </summary>
        PipeList Pipes { get; }

        /// <summary>
        /// List of all users within the current channel.
        /// </summary>
        UserList Users { get; }

        /// <summary>
        /// The primary message manager, responsible for creating 
        /// messages to be sent and recieved by varius network services.
        /// </summary>
        MessageManager<Byte> Messages { get; }
        #endregion
    }
}
