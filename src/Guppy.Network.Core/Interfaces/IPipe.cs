using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Network.Contexts;
using Guppy.Network.Lists;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface IPipe : INetworkService
    {
        #region Properties
        /// <summary>
        /// List of all <see cref="IUser"/> instances and any external
        /// <see cref="NetConnection"/> instances via <see cref="NetConnectionUserList.Connections"/>.
        /// </summary>
        UserList Users { get; }

        /// <summary>
        /// The owning <see cref="IChannel"/>.
        /// </summary>
        IChannel Channel { get; internal set; }
        #endregion

        #region Methods
        /// <summary>
        /// Create a new unsigned & typeless <see cref="NetOutgoingMessage"/> within the current <see cref="IPipe"/>.
        /// This should almost never be used unless defining a factory within <see cref="MessageManager.Add(uint, Func{NetOutgoingMessageContext, NetConnection, NetOutgoingMessage}, NetOutgoingMessageContext)"/>
        /// </summary>
        /// <returns></returns>
        NetOutgoingMessage CreateMessage(NetOutgoingMessageContext context, NetConnection recipient = null, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter = null);
        #endregion
    }
}
