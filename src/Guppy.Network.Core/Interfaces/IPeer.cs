using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection.Descriptors;
using Guppy.DependencyInjection;
using Guppy.Network.Lists;
using Guppy.Events.Delegates;
using Lidgren.Network;
using Guppy.Network.Contexts;
using Guppy.Network.Security;
using Guppy.Interfaces;

namespace Guppy.Network.Interfaces
{
    public interface IPeer : IService
    {
        #region Properties
        /// <summary>
        /// A list of all known users connected to the peer.
        /// </summary>
        UserList Users { get; }

        /// <summary>
        /// The primary <see cref="ChannelList"/> bound to this <see cref="IPeer"/>.
        /// </summary>
        ChannelList Channels { get; }

        /// <summary>
        /// The current user owned by the internal peer.
        /// </summary>
        IUser CurrentUser { get; }

        /// <summary>
        /// A collection of delegates that maybe bound to for raw message processing.
        /// </summary>
        Dictionary<NetIncomingMessageType, OnEventDelegate<IPeer, NetIncomingMessage>> OnIncomingMessageTypeRecieved { get; }

        /// <summary>
        /// Event invoked when any message is recieved.
        /// </summary>
        event OnEventDelegate<IPeer, NetIncomingMessage> OnIncomingMessageRecieved;

        /// <summary>
        /// The <see cref="ServiceConfigurationDescriptor.Key"/> to be used when creating a new
        /// <see cref="IChannel"/> instance.
        /// </summary>
        ServiceConfigurationKey ChannelServiceConfigurationKey { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Start the current <see cref="IPeer"/> & its underlying <see cref="NetPeer"/>.
        /// This will automatically update the peer asynchronously. Note, IChannel instances
        /// must be manually updated still.
        /// </summary>
        void Start();

        /// <summary>
        /// Create a new unattached user with the recieved claims.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        IUser CreateUser(params Claim[] claims);
        #endregion
    }
}
